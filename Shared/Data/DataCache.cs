using System.Collections.Concurrent;

namespace James.Shared.Data;

public static class DataCache
{
    static DataCache()
    {
        //If memory management is needed, start a background task to watch memory usage here.  YAGNI
    }

    private static readonly ConcurrentDictionary<string, CachedResult> _cachedResults = new();
    /// <summary>
    /// The number of cache usages before a scan to remove expired items is called.
    /// </summary>
    /// <remarks>Balance the need for memory management vs the performance impact of scanning the cache.</remarks>
    private const int CacheUsesUntilGarbageCollection = 20;
    private static int _cacheUses;

    public static async Task GetCacheOrLoadDataAsync(LoadItem loadItem)
    {
        if (_cachedResults.TryGetValue(loadItem.Key, out var cachedValue))
        {
            if (DateTime.Now <= cachedValue.CachedUntil)
            {
                //Cache hit
                loadItem.CacheLoadTask(_cachedResults[loadItem.Key].Data);
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
                Data = loadItem.ResultVariable().DataObject,
                CachedUntil = DateTime.Now + loadItem.CacheDuration
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

    ExpireCacheIfNeeded:
        if (0 == ++_cacheUses % CacheUsesUntilGarbageCollection)
            ReleaseExpired();
    }

    /// <summary>
    /// Load multiple items in parallel
    /// </summary>
    /// <param name="loadItems">The LoadItem to load</param>
    /// <returns></returns>
    public static async Task ParallelGetCacheOrDataAsync(params LoadItem[] loadItems)
    {
        await ParallelGetCacheOrDataAsync(null, loadItems);
    }

    /// <summary>
    /// Load multiple items in parallel and then run the afterAllLoaded when loading complete
    /// </summary>
    /// <param name="afterAllLoaded">Action to execute after all items have been loaded.</param>
    /// <param name="loadItems">The LoadItem to load</param>
    /// <returns></returns
    public static async Task ParallelGetCacheOrDataAsync(Action? afterAllLoaded, params LoadItem[] loadItems)
    {
        await Task.WhenAll(loadItems.Select(GetCacheOrLoadDataAsync));
        if (loadItems.All(li => li.ResultVariable().Success))
            afterAllLoaded?.Invoke();
    }

    /// <summary>
    /// Removes references to expired cache items
    /// </summary>
    private static void ReleaseExpired()
    {
        //This will run async and return immediately back to the calling function/
        Task.Factory.StartNew(() =>
        {
            if (_isReleasingExpired) return;
            try
            {
                _isReleasingExpired = true;
                var expired = _cachedResults.Where(cr => cr.Value.CachedUntil < DateTime.Now).Select(cr => cr.Key)
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

    private static bool _isReleasingExpired;
    private static object _releasingExpiredLockObject = new();

    /// <summary>
    /// Clears cache
    /// </summary>
    /// <remarks>Use cautiously as this clears the cache for the entire server if it is called server-side</remarks>
    public static void Clear()
    {
        _cachedResults.Clear();
    }

    /// <summary>
    /// Clears cache for one cache key
    /// </summary>
    /// <param name="key">Cache key to clear</param>
    public static void Clear(string key)
    {
        if (_cachedResults.TryGetValue(key, out var oldValue) && oldValue.Data is IDisposable disposeIt)
            disposeIt.Dispose();

        _cachedResults.Remove(key, out _);
    }
}