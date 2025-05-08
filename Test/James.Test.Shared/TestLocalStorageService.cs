using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace James.Test.Shared
{
    public class TestLocalStorageService: ILocalStorageService
    {
        private static ConcurrentDictionary<string, object> _testLocalStorage =
            new ConcurrentDictionary<string, object>();
        public async ValueTask ClearAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _testLocalStorage.Clear();
        }

        public async ValueTask<T> GetItemAsync<T>(string key,
            CancellationToken cancellationToken = new CancellationToken()) //where T : class 
        {
            _testLocalStorage.TryGetValue(key, out var value);
            return await Task.FromResult<T>((T?)value);
    }

        public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.FromResult( _testLocalStorage[key].ToString());
        }

        public async ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.FromResult(_testLocalStorage.Keys.ToArray()[index]);
        }

        public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.FromResult(_testLocalStorage.Keys);
        }

        public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.FromResult(_testLocalStorage.ContainsKey(key));
        }

        public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.FromResult(_testLocalStorage.Count); ;
        }

        public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = new CancellationToken())
        {
            _testLocalStorage.Remove(key, out _);
        }

        public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = new CancellationToken())
        {
            _testLocalStorage[key] = data;
        }

        public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = new CancellationToken())
        {
            _testLocalStorage[key] = data;
        }

        public event EventHandler<ChangingEventArgs>? Changing;
        public event EventHandler<ChangedEventArgs>? Changed;
    }
}
