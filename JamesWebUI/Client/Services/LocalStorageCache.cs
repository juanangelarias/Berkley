using Blazored.LocalStorage;
using James.Shared;
using James.Shared.Data;
using Microsoft.JSInterop;

namespace JamesWebUI.Client.Services
{
    public class BlazorLocalStorageCache(ILocalStorageService localStorageService, IJSRuntime js) : IBrowserStorageCache
    {
        private ILocalStorageService LocalStorageService => localStorageService;
        private IJSRuntime Js => js;
        public bool UseBrowserStorageCache => true;
        public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            await LocalStorageService.ClearAsync(cancellationToken);
        }

        public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return await LocalStorageService.GetItemAsync<T>(key, cancellationToken);
        }

        public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
        {
            return await LocalStorageService.GetItemAsStringAsync(key, cancellationToken);
        }

        public async ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
        {
            return await LocalStorageService.KeyAsync(index, cancellationToken);
        }

        public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
        {
            return await LocalStorageService.KeysAsync(cancellationToken);
        }

        public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return await LocalStorageService.ContainKeyAsync(key, cancellationToken);
        }

        public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
        {
            return await LocalStorageService.LengthAsync(cancellationToken);
        }

        public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
            await LocalStorageService.RemoveItemAsync(key, cancellationToken);
        }

        public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            await LocalStorageService.RemoveItemsAsync(keys, cancellationToken);
        }

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            await localStorageService.SetItemAsync(key, data, cancellationToken);
        }

        public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
        {
            await localStorageService.SetItemAsStringAsync(key, data, cancellationToken);
        }

        public async ValueTask<string[]> GetAllLocalStorageKeys(string startsWith = "")
        {
            var allKeys = await Js.InvokeAsync<string[]>("getAllLocalStorageKeys");
            if (null! == allKeys)
                return [];
            return string.IsNullOrEmpty(startsWith) ? allKeys : allKeys.Where(k => k.StartsWith(startsWith)).ToArray();
        }

        public async ValueTask SetItemAsyncWithExpiry<T>(string key, TimeSpan expiry, T data)
        {
            CachedResult storageItem = new CachedResult<T>
            {
                DataObject = data,
                CacheUntil = DateTime.UtcNow.Add(expiry)
            };
            await localStorageService.SetItemAsync(key, storageItem);
        }

        public async ValueTask<CachedResult<T>> GetCacheItem<T>(string key)
        {
            var storageItem = await localStorageService.GetItemAsync<CachedResult<T>>(key);

            if (storageItem is null)
            {
                return null!;
            }
            if (storageItem.CacheUntil < DateTime.UtcNow)
            {
                await localStorageService.RemoveItemAsync(key);
                return null!;
            }
            return storageItem;
        }

        public async ValueTask<T?> GetItemAsyncWithExpiry<T>(string key)
        {
            var cachedItem = await GetCacheItem<T>(key);
            return null! == cachedItem ? default : cachedItem.Data;
        }
    }
}
