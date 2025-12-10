using Blazored.LocalStorage;
using James.Shared.Data;
using JamesWebUI.Client.Shared;
using System.Diagnostics;
using James.Shared.Dto;
using James.Shared.Model;
using JamesWebUI.Client.Model;
using Newtonsoft.Json;

namespace JamesWebUI.Client.Services;

public interface IUserSettingService
{
    string? GridKey { get; set; }
    Task<Dictionary<string, string>> GetAllUserSettingsAsync(bool forceReload = false);
    Task<string?> GetUserSettingAsync(string key, bool forceReload = false);
    Task<ISaveDataResult> SetUserSettingAsync(string key, string? value);
    Task<ISaveDataResult> SetDefaultUserSettingAsync(string key, string? value);
}

public class UserSettingService(IDataAccess dataAccess, ILocalStorageService localStorageService) : IUserSettingService
{
    public string? GridKey { get; set; }
    
    #region Constants for used keys

    public const string SuperSearchSettingsKey = "SuperSearchSettings";

    #endregion

    private string CacheKey => $"UserSettings{Environment.UserName}{SubKey}";
    private string SubKey => GridKey == null ? "" : $"_{GridKey}";
    
    #region Loaders

    #region User Settings Load

    private IDataAccessResult<List<KeyValue>> _loadSettingsResult = null!;

    private LoadItem UserSettingLoadItem() =>
        new()
        {
            Key = CacheKey,
            AsyncLoadTask = async () => _loadSettingsResult = await dataAccess.GetAllUserSettings(),
            CacheLoadTask = cache => _loadSettingsResult = new DataAccessResult<List<KeyValue>>
                { Data = (List<KeyValue>)cache! },
            ResultVariable = () => _loadSettingsResult,
            AfterLoad = () =>
            {
                Debug.Assert(_loadSettingsResult.Data != null, "_loadSettingsResult.Data != null");
                //Add after load code here
                _settings = _loadSettingsResult.Data.ToDictionary(kv => kv.Key, kv => kv.Value);
                //Save to local storage asynchronously and don't wait for the save to finish
                Task.Factory.StartNew(data =>
                    localStorageService.SetItemAsyncWithExpiry(CacheKey, TimeSpan.FromDays(1), data), _settings);
            }
        };

    #endregion

    #endregion

    private Dictionary<string, string>? _settings;
    private Task? _userSettingsLoadTask;
    private readonly SemaphoreSlim _semaphore = new(1, 50);
    
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
        if(userSettings == null) 
            return null;
        
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