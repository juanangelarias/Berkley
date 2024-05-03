using System.Diagnostics;

namespace JamesWebUI.Server.Model
{
    public class UserInformationCache<T>
    {
        private Dictionary<string, CacheEntry<T>> _cache = new();

        public UserInformationCache()
        {
        }

        public UserInformationCache(Func<string, Task<T>>? lookupTask)
        {
            LookupTask = lookupTask;
        }

        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromMinutes(20);
        public Func<string, Task<T>>? LookupTask { get; init; }
        public async Task<T?> GetAsync(string key)
        {
            lock(this)
            RemoveStale();
            if (_cache.ContainsKey(key))
                Debug.WriteLine("Cache hit!");
            else if (null != LookupTask)
                _cache[key] = new CacheEntry<T>(await LookupTask(key));
            return _cache.ContainsKey(key) ? _cache[key].Value : default;
        }

        private void RemoveStale()
        {
            var stale = _cache.Where(ce => DateTime.Now - ce.Value.CachedAt > CacheDuration).ToArray();
            foreach (var entry in stale)
                _cache.Remove(entry.Key);
        }
    }

    public class CacheEntry<T>
    {
        public CacheEntry(T value) : this(value, DateTime.Now)
        {
        }

        public CacheEntry(T value, DateTime cachedAt)
        {
            Value = value;
            CachedAt = cachedAt;
        }
        public T Value { get; init; }
        public DateTime CachedAt { get; init; }
    }
}
