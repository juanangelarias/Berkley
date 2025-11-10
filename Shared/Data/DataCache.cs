using System.Collections;
using System.Collections.Concurrent;
using James.Shared.Constants;

namespace James.Shared.Data;

[Obsolete]
public interface IDataCache
{
    Task GetCacheOrLoadDataAsync(LoadItem loadItem, ILoggingService logger);
    Task GetCacheOrLoadDataAsync(LoadItem loadItem);

    /// <summary>
    /// Load multiple items in parallel
    /// </summary>
    /// <param name="loadItems">The LoadItem to load</param>
    Task ParallelGetCacheOrDataAsync(params LoadItem[] loadItems);

    /// <summary>
    /// Load multiple items in parallel and then run the afterAllLoaded when loading complete
    /// </summary>
    /// <param name="afterAllLoaded">Action to execute after all items have been loaded.</param>
    /// <param name="loadItems">The LoadItem to load</param>
    /// <remarks>Use this is you need multiple data sets before contracting a final output.</remarks>
    Task ParallelGetCacheOrDataAsync(Action? afterAllLoaded, params LoadItem[] loadItems);

    /// <summary>
    /// Clears cache
    /// </summary>
    /// <remarks>Use cautiously as this clears the cache for the entire server if it is called server-side</remarks>
    void Clear();

    /// <summary>
    /// Clears cache for one cache key
    /// </summary>
    /// <param name="key">Cache key to clear</param>
    void Clear(string key);

    /// <summary>
    /// Updates the cache with the specified key and data.
    /// </summary>
    /// <param name="key">The cache key to update.</param>
    /// <param name="data">The data to store in the cache.</param>
    /// <param name="cacheDuration">Optional cache duration. If not specified, a default duration may be applied.</param>
    void UpdateCache(string key, object data, TimeSpan? cacheDuration = null);
}
[Obsolete]
public class DataCache : IDataCache
{
    private readonly ConcurrentDictionary<string, CachedResult> _cachedResults = new();

    /// <summary>
    /// The number of cache usages before a scan to remove expired items is called.
    /// </summary>
    /// <remarks>Balance the need for memory management vs. the performance impact of scanning the cache.</remarks>
    private const int CacheUsesUntilGarbageCollection = 200;

    private int _cacheUses;
    private readonly Hashtable _executingLoadItems = new();
    private readonly Random _rnd = new();
    private ILoggingService? _logger;

    public async Task GetCacheOrLoadDataAsync(LoadItem loadItem, ILoggingService logger)
    {
        _logger = logger;
        await GetCacheOrLoadDataAsync(loadItem);
    }

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

            if (null != loadItem.LocalStorageCacheLoadTask)
                try
                {
                    //If a local storage cache is available, load from it rather than the database
                    object? fromLocalCache;
                    if (null != (fromLocalCache = await loadItem.LocalStorageCacheLoadTask()))
                    {
                        _cachedResults[loadItem.Key] = new CachedResult
                            { DataObject = fromLocalCache, CacheUntil = DateTime.Now + loadItem.CacheDuration };
                        //Once the cache has been set, continue like it was a cache hit so that the load result is properly set
                        loadItem.CacheLoadTask(_cachedResults[loadItem.Key].DataObject);
                        //Run AfterLoad as though data was just loaded
                        loadItem.AfterLoad?.Invoke();
                        goto ExpireCacheIfNeeded; //<Evil grin>A goto statement!</Evil grin>
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogException(ex, "Exception trying to load from LocalStorage",
                        category: StandardLoggingCategories.BrowserFeatures,
                        data: new Dictionary<string, string> { { "Key", loadItem.Key } });
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
                loadItem.FireLoaded();
                loadItem.AfterLoad?.Invoke();
                loadItem.AddSubscriptionTask?.Invoke(loadItem);
            }
            else
            {
                var errorList = new List<string>(loadItem.ResultVariable().Errors);
                errorList.Add("Maximum number of retries exceeded.");
                loadItem.LoadErrorsEncountered(errorList.ToArray(), true);
            }

            ExpireCacheIfNeeded: //<Evil grin>Label used by a goto statement!</Evil grin>

            if (0 == ++_cacheUses % CacheUsesUntilGarbageCollection)
                ReleaseExpired();
        }
        finally
        {
            //TODO:Unlock
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
        //This will run async and return immediately to the calling function/
        Task.Factory.StartNew(() =>
        {
            if (_isReleasingExpired)
                //Quit if a previous ReleaseExpired() is still running
                return;
            lock (_releasingExpiredLockObject)
                try
                {
                    _isReleasingExpired = true;
                    var expired = _cachedResults.Where(cr => cr.Value.CacheUntil < DateTime.Now).Select(cr => cr.Key)
                        .ToArray();
                    foreach (var expiredCacheItemKey in expired)
                        Clear(expiredCacheItemKey);
                }
                finally
                {
                    _isReleasingExpired = false;
                }
        });
    }

    private bool _isReleasingExpired;
    private readonly object _releasingExpiredLockObject = new();

    /// <summary>
    /// Clears cache
    /// </summary>
    /// <remarks>Use cautiously as this clears the cache for the entire server if it is called server-side</remarks>
    public void Clear()
    {
        _cachedResults.Clear();
    }

    /// <summary>
    /// Clears cache for one cache key
    /// </summary>
    /// <param name="key">Cache key to clear</param>
    public void Clear(string key)
    {
        if (_cachedResults.TryGetValue(key, out var oldValue) && oldValue.DataObject is IDisposable disposeIt)
            disposeIt.Dispose();

        _cachedResults.Remove(key, out _);
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

    public void UpdateCache(string key, object data, TimeSpan? cacheDuration = null)
    {
        cacheDuration ??= TimeSpan.FromHours(1);
        var newData = new CachedResult
        {
            DataObject = data,
            CacheUntil = DateTime.Now + cacheDuration.Value
        };
        
        if (_cachedResults.TryGetValue(key, out _))
            _cachedResults.TryRemove(key, out _);
        
        _cachedResults.TryAdd(key, newData);
    }
}