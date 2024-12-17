using System.Collections.Concurrent;

namespace James.Shared.Data;

public class DataCache
{
    static DataCache()
    {
        //If memory management is needed, start a background task to watch memory usage here.  YAGNI
    }

    private static readonly ConcurrentDictionary<string, CachedResult> _cachedResults = new();

    public static async Task GetCacheOrLoadDataAsync(LoadItem loadItem)
    {
        if (_cachedResults.TryGetValue(loadItem.Key, out var cachedValue))
        {
            if ((DateTime.Now - cachedValue.CachedTime) <= loadItem.CacheDuration)
            {
                //Cache hit
                loadItem.CacheLoadTask(_cachedResults[loadItem.Key].Data);
                return;
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
            _cachedResults[loadItem.Key] = new CachedResult { Data = loadItem.ResultVariable().DataObject };
            loadItem.FireLoaded(); //TODO:Set up subscriptions to keep data updated.
        }
        else
        {
            var errorList = new List<string>(loadItem.ResultVariable().Errors);
            errorList.Add("Maximum number of retries exceeded.");
            loadItem.LoadErrorsEncountered(errorList.ToArray(), true);
        }
    }

    public static async Task ParallelGetCacheOrDataAsync(params LoadItem[] loadItems)
    {
        await Task.WhenAll(loadItems.Select(GetCacheOrLoadDataAsync));
    }
}