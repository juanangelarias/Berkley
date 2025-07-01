using System.Collections;
using System.Collections.Concurrent;

namespace James.Shared.Data;

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
}

public class DataCache : IDataCache
{
    private DataCache()
    {
        //If memory management is needed, start a background task to watch memory usage here.  YAGNI
    }

    private readonly ConcurrentDictionary<string, CachedResult> _cachedResults = new();

    /// <summary>
    /// The number of cache usages before a scan to remove expired items is called.
    /// </summary>
    /// <remarks>Balance the need for memory management vs the performance impact of scanning the cache.</remarks>
    private const int CacheUsesUntilGarbageCollection = 200;

    private int _cacheUses;
    private Hashtable _executingLoadItems = new();
    private Random _rnd = new();
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
            if (_cachedResults.TryGetValue(loadItem.Key, out var cachedValue))
            {
                if (DateTime.Now <= cachedValue.CacheUntil)
                {
                    //Cache hit
                    loadItem.CacheLoadTask(_cachedResults[loadItem.Key].DataObject);
                    //Run AfterLoad as though data was just loaded
                    loadItem.AfterLoad?.Invoke();
                    goto ExpireCacheIfNeeded; //<Evil grin>A goto statement!</Evil grin>
                }

                //Cache is expired
                _cachedResults.TryRemove(loadItem.Key, out _);
            }

            if (null != loadItem.LocalStorageCacheLoadTask)
                try
                {
                    //If a local storage cache is available, load from it rather than the database
                    object? fromLocalCache;
                    if (null != (fromLocalCache = await loadItem.LocalStorageCacheLoadTask()))
                    {
                        _cachedResults[loadItem.Key] = new CachedResult
                            { DataObject = fromLocalCache, CacheUntil = DateTime.Now + loadItem.CacheDuration };
                        //Once cache has been set, continue like it was a cache hit so that the load result is properly set
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
        //This will run async and return immediately back to the calling function/
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
}