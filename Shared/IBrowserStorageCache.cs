using James.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared
{
    public interface IBrowserStorageCache
    {
        /// <summary>
        /// Whether this cache should be used
        /// </summary>
        /// <returns>True to use the cache, false to bypass this cache</returns>
        bool UseBrowserStorageCache { get; }

        /// <summary>
        /// Clears all data from local storage.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask ClearAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the specified data from local storage and deserialize it to the specified type.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the local storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the specified data from local storage as a <see cref="string"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Return the name of the key at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a collection of strings representing the names of the keys in the local storage.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if the <paramref name="key"/> exists in local storage, but does not check its value.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// The number of items stored in local storage.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask<int> LengthAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove the data with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a collection of <paramref name="keys"/>.
        /// </summary>
        /// <param name="keys">A IEnumerable collection of strings specifying the name of the storage slot to remove</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The data to be saved</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns>A <see cref="ValueTask"/> representing the completion of the operation.</returns>
        ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets or updates the <paramref name="data"/> in local storage with the specified <paramref name="key"/>. Does not serialize the value before storing.
        /// </summary>
        /// <param name="key">A <see cref="string"/> value specifying the name of the storage slot to use</param>
        /// <param name="data">The string to be saved</param>
        /// <param name="cancellationToken">
        /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
        /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
        /// </param>
        /// <returns></returns>
        ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of all keys with values in the browser storage cache.
        /// </summary>
        /// <param name="startsWith">Only return keys starting with this prefix</param>
        /// <returns>String[] of all exiting keys that match the filter.</returns>
        ValueTask<string[]> GetAllLocalStorageKeys(string startsWith = "");

        ValueTask SetItemAsyncWithExpiry<T>(string key, TimeSpan expiry, T data);
        ValueTask<CachedResult<T>> GetCacheItem<T>(string key);
        ValueTask<T?> GetItemAsyncWithExpiry<T>(string key);
    }

    /// <summary>
    /// Concrete class for use in non-browser environments
    /// </summary>
    public class NoBrowserStorageCache : IBrowserStorageCache
    {
        public bool UseBrowserStorageCache => false;
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
        }

        public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            return default(T);
        }

        public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
        {
            return null!;
        }

        public async ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default)
        {
            return null!;
        }

        public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
        {
            return null!;
        }

        public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return false;
        }

        public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
        {
            return 0;
        }

        public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
        }

        public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
        }

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
        }

        public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
        {
        }

        public async ValueTask<string[]> GetAllLocalStorageKeys(string startsWith = "")
        {
            return [];
        }

        public async ValueTask SetItemAsyncWithExpiry<T>(string key, TimeSpan expiry, T data)
        {
        }

        public async ValueTask<CachedResult<T>> GetCacheItem<T>(string key)
        {
            return null!;
        }

        public async ValueTask<T?> GetItemAsyncWithExpiry<T>(string key)
        {
            return default;
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
