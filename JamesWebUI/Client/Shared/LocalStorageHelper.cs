using Blazored.LocalStorage;
using James.Shared.Data;

namespace JamesWebUI.Client.Shared
{
    [Obsolete]
    public static class LocalStorageHelper
    {
        public static async Task SetItemAsyncWithExpiry<T>(this ILocalStorageService localStorageService, string key, TimeSpan expiry, T data)
        {
            CachedResult storageItem = new CachedResult<T>
            {
                DataObject = data,
                CacheUntil = DateTime.UtcNow.Add(expiry)
            };
            await localStorageService.SetItemAsync(key, storageItem);
        }

        public static async Task<T?> GetItemAsyncWithExpiry<T>(this ILocalStorageService localStorageService, string key)
        {
            var storageItem = await localStorageService.GetItemAsync<CachedResult<T>>(key);

            if (storageItem is null)
            {
                return default;
            }

            if (storageItem.CacheUntil < DateTime.UtcNow)
            {
                await localStorageService.RemoveItemAsync(key);
                return default;
            }
            return storageItem.Data;
        }
    }
}
