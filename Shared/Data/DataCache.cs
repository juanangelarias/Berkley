using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;

namespace James.Shared.Data;

public static class DataCache
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
                //Run AfterLoad as though data was just loaded
                loadItem.AfterLoad?.Invoke();
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
            loadItem.AfterLoad?.Invoke();
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
        await ParallelGetCacheOrDataAsync(null, loadItems);
    }
    public static async Task ParallelGetCacheOrDataAsync(Action? afterAllLoaded, params LoadItem[] loadItems)
    {
        await Task.WhenAll(loadItems.Select(GetCacheOrLoadDataAsync));
        if (loadItems.All(li=>li.ResultVariable().Success))
        {
            //TODO:Remove after debugging
            var sb = new StringBuilder($"{loadItems.Length} load items have completed.\r\n");
            for (int i = 1; i <= loadItems.Length;i++)
            {
                sb.Append("Task ");
                sb.Append(i.ToString("D2"));
                sb.Append(" key = '");
                sb.Append(loadItems[i-1].Key);
                sb.Append("', Success = ");
                sb.Append(loadItems[i-1].ResultVariable().Success.ToString());
                sb.Append(", Value is null = ");
                sb.AppendLine((loadItems[i-1].ResultVariable().DataObject == null).ToString());
            }

            var textSummary = sb.ToString();
            if (textSummary.Contains("false"))
                Debug.WriteLine("Break here");

            afterAllLoaded?.Invoke();
        }
    }

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
        _cachedResults.Remove(key, out _);
    }
}