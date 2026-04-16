using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;
using James.Shared.Constants;

namespace James.Shared.Data
{
    public abstract class BaseDataAccess(IBrowserStorageCache browserStorageCache, ILoggingService logger)
    {
        private readonly ConcurrentDictionary<string, CachedResult> _cachedResults = new();

        /// <summary>
        /// Reference to browser storage cache, or an instance of NoBrowserStorageCache in non-browser environments
        /// </summary>
        public IBrowserStorageCache BrowserStorageCache => browserStorageCache;

        /// <summary>
        /// The number of cache usages before a scan to remove expired items is called.
        /// </summary>
        /// <remarks>Balance the need for memory management vs. the performance impact of scanning the cache.</remarks>
        private const int CacheUsesUntilGarbageCollection = 200;

        private int _cacheUses;
        private readonly Hashtable _executingLoadItems = new();
        private readonly Random _rnd = new();

        public async Task GetCacheOrLoadDataAsync(LoadItem loadItem)
        {
            //TODO:  Need to fully test having requests wait if the loadItem is already in the middle of loading.  
            //          Perhaps this needs to implement Lazy<T> or even
            //              AsyncLazy<T> from https://devblogs.microsoft.com/pfxteam/asynclazyt/
            while (_executingLoadItems.ContainsKey(loadItem.Key))
                await Task.Delay(12 + _rnd.Next(18));
            try
            {
                _executingLoadItems.Add(loadItem.Key, 1);

                if (loadItem.UseBrowserStorageIfAvailable)
                    try
                    {
                        //If a local storage cache is available, load from it rather than the database
                        if (BrowserStorageCache.UseBrowserStorageCache)
                        {
                            var localCachedValue = await BrowserStorageCache.GetCacheItem<object>(loadItem.Key);
                            while (localCachedValue != null!) //NOTE: Using while instead of if to allow the use of break; to leave early
                            {
                                if (localCachedValue.DataObject is JsonElement jElement )
                                {
                                    if (loadItem.GetType().IsGenericType)
                                    {
                                        var genType = loadItem.GetType().GenericTypeArguments.First();
                                        localCachedValue.DataObject =
                                            JsonSerializer.Deserialize(jElement.GetRawText(), genType);
                                    }
                                    else
                                    {
                                        //Continue like no cache is available, but leave warning in log.
                                        logger.LogWarning("LoadItem should be upgraded to LoadItem<T>",
                                            category: StandardLoggingCategories.DataAccess,
                                            data: new Dictionary<string, string>
                                            {
                                                { "loadItemKey", loadItem.Key }
                                            });
                                        break;
                                    }
                                }
                                _cachedResults[loadItem.Key] = localCachedValue;
                                //Once the cache has been set, continue like it was a cache hit so that the load result is properly set
                                loadItem.CacheLoadTask(_cachedResults[loadItem.Key].DataObject);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                       logger.LogException(ex, "Exception trying to load from LocalStorage",
                            category: StandardLoggingCategories.BrowserFeatures,
                            data: new Dictionary<string, string> { { "Key", loadItem.Key } });
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                       BrowserStorageCache.ClearAsync(); //Make sure that the local cache doesn't have a poison pill
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                       throw;
                    }
                if (_cachedResults.TryGetValue(loadItem.Key, out var cachedValue))
                {
                    if (DateTime.Now <= cachedValue.CacheUntil)
                    {
                        //Cache hit
                        loadItem.CacheLoadTask(_cachedResults[loadItem.Key].DataObject);
                        //Run AfterLoad as though data was just loaded
                        loadItem.AfterLoad?.Invoke();
                        goto ExpireCacheIfNeeded;//<Evil grin>A goto statement!</Evil grin>
                    }

                    //Cache is expired
                    _cachedResults.TryRemove(loadItem.Key, out _);
                }

                var retries = JamesConstants.Default_Max_Retries;
                await loadItem.AsyncLoadTask();
                while (!loadItem.ResultVariable().Success && retries-- > 0)
                {
                    loadItem.LoadErrorsEncountered(loadItem.ResultVariable().Errors, false);
                    //Pause and retry
                    await Task.Delay((int)Math.Pow(JamesConstants.Default_Max_Retries - retries, 2) * 250);
                    await loadItem.AsyncLoadTask();
                }

                if (loadItem.ResultVariable().Success)
                {
                    _cachedResults[loadItem.Key] = new CachedResult
                    {
                        DataObject = loadItem.ResultVariable().DataObject,
                        CacheUntil = DateTime.Now + loadItem.CacheDuration
                    };
                    if (loadItem.UseBrowserStorageIfAvailable)
                    {
                        //Update local browser storage cache without waiting for it to complete.
                        _ = BrowserStorageCache.SetItemAsyncWithExpiry(loadItem.Key, loadItem.CacheDuration,
                                                                        loadItem.ResultVariable().DataObject);
                    }
                    loadItem.FireLoaded();
                    loadItem.AfterLoad?.Invoke();
                    loadItem.AddSubscriptionTask?.Invoke(loadItem);
                }
                else
                {
                    var errorList = new List<string>(loadItem.ResultVariable().Errors)
                    {
                        "Maximum number of retries exceeded."
                    };
                    loadItem.LoadErrorsEncountered(errorList.ToArray(), true);
                }

                ExpireCacheIfNeeded: //<Evil grin>Label used by a goto statement!</Evil grin>

                if (0 == ++_cacheUses % CacheUsesUntilGarbageCollection)
                    ReleaseExpired();
            }
            finally
            {
                //Unlock
                _executingLoadItems.Remove(loadItem.Key);
            }
        }

        /// <summary>
        /// Load multiple items in parallel
        /// </summary>
        /// <param name="loadItems">The LoadItem to load</param>
        public async Task ParallelGetCacheOrDataAsync(params LoadItem[] loadItems)
        {
            await ParallelGetCacheOrDataAsync(null, loadItems);
        }

        /// <summary>
        /// Load multiple items in parallel and then run the afterAllLoaded when loading complete
        /// </summary>
        /// <param name="afterAllLoaded">Action to execute after all items have been loaded.</param>
        /// <param name="loadItems">The LoadItem to load</param>
        /// <remarks>Use this is you need multiple data sets before contracting a final output.</remarks>
        public async Task ParallelGetCacheOrDataAsync(Action? afterAllLoaded, params LoadItem[] loadItems)
        {
            await Task.WhenAll(loadItems.Select(GetCacheOrLoadDataAsync));
            if (loadItems.All(li => li.ResultVariable().Success))
                afterAllLoaded?.Invoke();
        }

        /// <summary>
        /// Removes references to expired cache items
        /// </summary>
        private void ReleaseExpired()
        {
            if (_isReleasingExpired)
                //Quit if a previous ReleaseExpired() is still running
                return;

            _isReleasingExpired = true;

            //This will run async and return immediately to the calling function/
            _ = Task.Run(async () =>
            {
                try
                {
                    var expired = _cachedResults.Where(cr => cr.Value.CacheUntil < DateTime.Now).Select(cr => cr.Key)
                        .ToArray();
                    foreach (var expiredCacheItemKey in expired)
                        await ClearAsync(expiredCacheItemKey);
                }
                catch (Exception exception)
                {
                    logger.LogException(exception, "Exception during background cache expiration",
                        category: StandardLoggingCategories.DataAccess);
                }
                finally
                {
                    _isReleasingExpired = false;
                }
            });
        }

        private bool _isReleasingExpired;

        /// <summary>
        /// Clears cache
        /// </summary>
        /// <remarks>Use cautiously as this clears the cache for the entire server if it is called server-side</remarks>
        public async Task ClearAsync()
        {
            _cachedResults.Clear();

            if (BrowserStorageCache.UseBrowserStorageCache)
                await BrowserStorageCache.ClearAsync();
        }

        /// <summary>
        /// Clears cache for one cache key
        /// </summary>
        /// <param name="key">Cache key to clear</param>
        public async Task ClearAsync(string key)
        {
            if (_cachedResults.TryGetValue(key, out var oldValue) && oldValue.DataObject is IDisposable disposeIt)
                disposeIt.Dispose();

            _cachedResults.Remove(key, out _);

            if (BrowserStorageCache.UseBrowserStorageCache)
                await BrowserStorageCache.RemoveItemAsync(key);
        }

        /// <summary>
        /// Updates or adds an item to the cache with an optional cache duration.
        /// </summary>
        /// <param name="key">The unique identifier for the cached item.</param>
        /// <param name="data">The data object to be cached.</param>
        /// <param name="cacheDuration">
        /// Optional duration for which the item should remain in the cache. 
        /// Defaults to 1 hour if not specified.
        /// </param>
        /// <remarks>
        /// If the key already exists in the cache, the existing entry is updated.
        /// If the key does not exist, a new cache entry is created.
        /// </remarks>
        public async Task UpdateCacheAsync(string key, object data, TimeSpan? cacheDuration = null)
        {
            cacheDuration ??= TimeSpan.FromHours(1);
            var newData = new CachedResult
            {
                DataObject = data,
                CacheUntil = DateTime.Now + cacheDuration.Value
            };

            await ClearAsync(key);
            
            _cachedResults.TryAdd(key, newData);
            await BrowserStorageCache.SetItemAsyncWithExpiry(key, cacheDuration.Value, data);
        }
    }
}
