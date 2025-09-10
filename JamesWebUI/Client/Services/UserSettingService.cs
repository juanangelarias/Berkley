using Blazored.LocalStorage;
using James.Shared.Data;
using JamesWebUI.Client.Shared;
using System.Diagnostics;

namespace JamesWebUI.Client.Services
{
    public interface IUserSettingService
    {
        Task<Dictionary<string, string>> GetAllUserSettingsAsync(bool forceReload = false);
        Task<string?> GetUserSettingAsync(string key, bool forceReload = false);
        Task<ISaveDataResult> SetUserSettingAsync(string key, string? value);
        Task<ISaveDataResult> SetDefaultUserSettingAsync(string key, string? value);
    }

    public class UserSettingService(IDataAccess dataAccess,
        ILocalStorageService localStorageService) : IUserSettingService
    {
        #region Constants for used keys
        public const string SuperSearchSettingsKey = "SuperSearchSettings";
        #endregion

        private string CacheKey => $"UserSettings{Environment.UserName}";
        private IDataAccessResult<Dictionary<string, string>> _loadSettingsResult = null!;
        private LoadItem UserSettingLoadItem() =>
            new()
            {
                Key = CacheKey,
                AsyncLoadTask = async () => _loadSettingsResult = await dataAccess.GetAllUserSettings(),
                CacheLoadTask = cache => _loadSettingsResult = new DataAccessResult<Dictionary<string, string>> { Data = (Dictionary<string, string>)cache! },
                ResultVariable = () => _loadSettingsResult,
                AfterLoad = () =>
                {
                    Debug.Assert(_loadSettingsResult.Data != null, "_loadSettingsResult.Data != null");
                    //Add after load code here
                    _settings = _loadSettingsResult.Data;
                    //Save to local storage asynchronously and don't wait for the save to finish
                    Task.Factory.StartNew(data =>
                        localStorageService.SetItemAsyncWithExpiry(CacheKey, TimeSpan.FromDays(1), data), _settings);
                }
            };

        private Dictionary<string, string>? _settings;
        private Task? _userSettingsLoadTask;
        private async Task<Dictionary<string, string>> GetUserSettings()
        {
            if (null == _settings)
                await _semaphore.WaitAsync();
            try
            {
                await (_userSettingsLoadTask ??= dataAccess.GetCacheOrLoadDataAsync(UserSettingLoadItem()));
            }
            finally
            {
                _semaphore.Release();
            }
            return _settings!;
        }

        private readonly SemaphoreSlim _semaphore = new(1, 50);

        public async Task<Dictionary<string, string>> GetAllUserSettingsAsync(bool forceReload = false)
        {
            if (forceReload)
            {
                _settings = null;
                await localStorageService.RemoveItemAsync(CacheKey);
                dataAccess.Clear(CacheKey);
                await _semaphore.WaitAsync();
                try
                {
                    _loadSettingsResult = null!;
                    _userSettingsLoadTask = null;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            var userSettings = await GetUserSettings();
            return userSettings;
        }
        public async Task<string?> GetUserSettingAsync(string key, bool forceReload = false)
        {
            var userSettings = await GetAllUserSettingsAsync(forceReload);
            return userSettings.GetValueOrDefault(key);
        }
        public async Task<ISaveDataResult> SetUserSettingAsync(string key, string? value)
        {
            if (null != _settings)
            {
                //Alter local cache
                if (value != null)
                    _settings[key] = value;
                else _settings.Remove(key);
            }
            return await dataAccess.SetUserSetting(key, value);
        }
        public async Task<ISaveDataResult> SetDefaultUserSettingAsync(string key, string? value)
        {
            return await dataAccess.SetDefaultUserSetting(key, value);
        }
    }
}
