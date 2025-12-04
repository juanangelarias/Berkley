using Blazored.LocalStorage;
using James.Shared.Data;
using JamesWebUI.Client.Shared;
using System.Diagnostics;
using James.Shared.Model;
using JamesWebUI.Client.Model;
using Newtonsoft.Json;

namespace JamesWebUI.Client.Services;

public interface IUserSettingService: IDisposable
{
    string? GridKey { get; set; }
    Task<Dictionary<string, string>> GetAllUserSettingsAsync(bool forceReload = false);
    Task<string?> GetUserSettingAsync(string key, bool forceReload = false);
    Task<ISaveDataResult> SetUserSettingAsync(string key, string? value);
    Task<ISaveDataResult> SetDefaultUserSettingAsync(string key, string? value);
    Task<GridSettings?> GetGridSettings();
    Task<ISaveDataResult> SetGridSettings(GridSettings settings);
    Task<ISaveDataResult> ResetUserSettings();
}

public class UserSettingsService(IDataAccess dataAccess, ILocalStorageService localStorageService) : IUserSettingService
{
    
    public string? GridKey { get; set; }
    
    #region Constants for used keys

    public const string SuperSearchSettingsKey = "SuperSearchSettings";

    #endregion

    private string CacheKey => $"UserSettings{Environment.UserName}{SubKey}";
    private string SubKey => GridKey == null ? "" : $"_{GridKey}";
    
    #region Loaders

    #region User Settings Load

    private IDataAccessResult<Dictionary<string, string>> _loadSettingsResult = null!;

    private LoadItem UserSettingLoadItem =>
        new()
        {
            Key = CacheKey,
            AsyncLoadTask = async () => _loadSettingsResult = await dataAccess.GetAllUserSettings(),
            CacheLoadTask = cache => _loadSettingsResult = new DataAccessResult<Dictionary<string, string>>
                { Data = (Dictionary<string, string>)cache! },
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

    #endregion

    #region GridUserSettings Load

    private IDataAccessResult<UserSetting?> _gridUserSettingsResult = null!;
    private GridSettings? _gridSettings;

    private LoadItem GridUserSettingsLoad => new LoadItem<UserSetting?>
    {
        Key = CacheKey,
        AsyncLoadTask = async () =>
            _gridUserSettingsResult = await dataAccess.GetUserSetting(GridKey!),
        CacheLoadTask = cache =>
        {
            _gridUserSettingsResult = new DataAccessResult<UserSetting?>
            {
                Data = (UserSetting?)cache!
            };
        },
        ResultVariable = () => _gridUserSettingsResult,
        AfterLoad = () =>
        {
            if (_gridUserSettingsResult.Data == null)
                return;

            var userGridSettings = JsonConvert.DeserializeObject<GridSettings>(_gridUserSettingsResult.Data!.Value);

            _gridSettings = userGridSettings;
        },
        CacheDuration = new TimeSpan(0)
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
            await (_userSettingsLoadTask ??= dataAccess.GetCacheOrLoadDataAsync(UserSettingLoadItem));
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

        var userSetting = await GetUserSettings();
        return userSetting;
    }

    public async Task<string?> GetUserSettingAsync(string key, bool forceReload = false)
    {
        var userSetting = await GetAllUserSettingsAsync(forceReload);
        return userSetting.GetValueOrDefault(key);
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

    public async Task<GridSettings?> GetGridSettings()
    {
        if(string.IsNullOrWhiteSpace(GridKey))
            throw new ArgumentException("Key cannot be null or whitespace", nameof(GridKey));
        
        await dataAccess.GetCacheOrLoadDataAsync(GridUserSettingsLoad);
        
        return _gridSettings;
    }

    public async Task<ISaveDataResult> SetGridSettings(GridSettings settings)
    {
        if(string.IsNullOrWhiteSpace(GridKey))
            throw new ArgumentException("Key cannot be null or whitespace", nameof(GridKey));
        
        var value = JsonConvert.SerializeObject(settings);
        var deleteResponse = await dataAccess.ResetUserSetting(GridKey);
        if(!deleteResponse.Success)
            throw new InvalidOperationException("Unable to reset user settings before saving new settings.");
        
        var response = await dataAccess.SetUserSetting(GridKey, value);
        dataAccess.Clear(GridKey);
        return response;
    }

    public async Task<ISaveDataResult> ResetUserSettings()
    {
        if(string.IsNullOrWhiteSpace(GridKey))
            throw new ArgumentException("Key cannot be null or whitespace", nameof(GridKey));
        
        var response = await dataAccess.ResetUserSetting(GridKey);
        dataAccess.Clear(GridKey);
        
        return response;
    }
    
    public void Dispose()
    {
        _userSettingsLoadTask?.Dispose();
        _semaphore.Dispose();
    }
}