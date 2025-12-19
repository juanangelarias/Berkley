using Blazored.LocalStorage;
using James.Shared.Constants;
using James.Shared.Data;
using JamesWebUI.Client.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using Blazorise;

namespace JamesWebUI.Client.Services;

public interface IUserSettingService
{
    string? GridKey { get; set; }
    Task<Dictionary<string, string>> GetAllUserSettingsAsync(bool forceReload = false);
    Task<string?> GetUserSettingAsync(string key, bool forceReload = false);
    Task<ISaveDataResult> SetUserSettingAsync(string key, string? value);
    Task<ISaveDataResult> SetDefaultUserSettingAsync(string key, string? value);
}

public class UserSettingService(IDataAccess dataAccess, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider) : IUserSettingService
{
    public string? GridKey { get; set; }
    private string CacheKey(string username) => CacheKeys.UserSettings(username, GridKey);
    
    #region Constants for used keys

    public const string SuperSearchSettingsKey = "SuperSearchSettings";

    #endregion

    #region Loaders

    #region User Settings Load

    private IDataAccessResult<Dictionary<string, string>> _loadSettingsResult = null!;

    private LoadItem<Dictionary<string, string>> UserSettingLoadItem(string username) =>
        new()
        {
            Key = CacheKey(username),
            AsyncLoadTask = async () => _loadSettingsResult = await dataAccess.GetAllUserSettings(),
            CacheLoadTask = cache => _loadSettingsResult = new DataAccessResult<Dictionary<string, string>>
            { Data = (Dictionary<string, string>)cache! },
            ResultVariable = () => _loadSettingsResult,
            AfterLoad = () =>
            {
                Debug.Assert(_loadSettingsResult.Data != null);
                //Add after load code here
                _settings = _loadSettingsResult.Data;
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
            var username = await GetUserName();
            if (null == username) return null!;
            await (_userSettingsLoadTask ??= dataAccess.GetCacheOrLoadDataAsync(UserSettingLoadItem(username)));
        }
        finally
        {
            _semaphore.Release();
        }
        return _settings!;
    }


    private async Task<string?> GetUserName()
    {
        var username = Environment.UserName;
        if (username == "Browser")
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var retries = 20;
            while (authState.User.Identity?.IsAuthenticated != true && retries-- > 0)
            {
                await Task.Delay(100);
                authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            }
            if (retries < 20)
            {
                Console.WriteLine($"UserSettingService: Waited {20 - retries}*100ms for authentication state.");
            }
            var currentClaimsPrincipal = authState.User;
            username = currentClaimsPrincipal.FindFirst("nickname")?.Value
                       ?? currentClaimsPrincipal
                           .FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                           ?.Value;
        }
        return username;
    }

    public async Task<Dictionary<string, string>> GetAllUserSettingsAsync(bool forceReload = false)
    {
        var username = await GetUserName();
        var cacheKey = CacheKey(username!);
        if (forceReload)
        {
            _settings = null;
            await localStorageService.RemoveItemAsync(cacheKey);
            dataAccess.Clear(cacheKey);
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
        if (userSettings == null!)
            return null;

        return userSettings.GetValueOrDefault(key);
    }

    public async Task<ISaveDataResult> SetUserSettingAsync(string key, string? value)
    {
        if (null != _settings)
        {
            var username = await GetUserName();
            var cacheKey = CacheKey(username!);
            //Alter local cache
            if (value != null)
                _settings[key] = value;
            else _settings.Remove(key);
            dataAccess.UpdateCache(cacheKey, _settings);
            //Update the browser cache asynchronously
            localStorageService.SetItemAsyncWithExpiry(cacheKey, TimeSpan.FromDays(1), _settings);
        }
        return await dataAccess.SetUserSetting(key, value);
    }

    public async Task<ISaveDataResult> SetDefaultUserSettingAsync(string key, string? value)
    {
        if (null != _settings)
        {
            var username = await GetUserName();
            var cacheKey = CacheKey(username!);
            //Invalidate cache
            _settings = null;
            dataAccess.Clear(key);
            //Update the browser cache asynchronously
            localStorageService.RemoveItemAsync(cacheKey);
        }

        return await dataAccess.SetDefaultUserSetting(key, value);
    }
}