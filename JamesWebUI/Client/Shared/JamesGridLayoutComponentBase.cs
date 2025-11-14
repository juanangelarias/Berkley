using James.Shared.Constants;
using James.Shared.Data;
using James.Shared.Model;
using JamesWebUI.Client.Model;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

public abstract class JamesGridLayoutComponentBase<T> : JamesLayoutComponentBase
    where T : class
{
    [Inject] public required IDataAccess DataAccess { get; set; }
    
    #region Fields & Properties

    protected ExportFormat DefaultExportFormat = ExportFormat.Excel;
    protected bool UserSettingsLoaded;
    protected bool UserSettingsChanged;
    private DataGridSettings? _dataGridSettings;

    protected DataGridSettings? GridSettings
    {
        get => _dataGridSettings;
        set
        {
            if (_dataGridSettings == value)
                return;

            _dataGridSettings = value;
            UserSettingsChanged = true;
        }
    }

    protected RadzenDataGrid<T> Grid { get; set; } = null!;

    #endregion

    #region Loaders

    #region UserSettings Load

    private IDataAccessResult<UserSetting?> _userSettingsResult = null!;

    private LoadItem UserSettingsLoad => AddEventNotify(new LoadItem<UserSetting>
    {
        Key = "UserSettings",
        AsyncLoadTask = async () =>
            _userSettingsResult = await DataAccess.GetUserSetting(UserSettingsKeyConstants.AgencyAccountGrid),
        CacheLoadTask = cache => _userSettingsResult = new DataAccessResult<UserSetting?>
        {
            Data = (UserSetting?)cache!
        },
        ResultVariable = () => _userSettingsResult,
        AfterLoad = () =>
        {
            if (_userSettingsResult.Data == null)
                return;

            var userGridSettings = JsonConvert.DeserializeObject<GridSettings>(_userSettingsResult.Data!.Value);

            if (userGridSettings == null)
                return;

            DefaultExportFormat = userGridSettings.DefaultExportFormat;
            GridSettings = userGridSettings.Settings;
            UserSettingsLoaded = true;
            UserSettingsChanged = false;
        },
        CacheDuration = new TimeSpan(0, 0, 0, 0, 500)
    }, "user settings");

    #endregion

    #endregion

    protected async Task SaveGridSettings()
    {
        await ShowLoading();

        var userGridSettings = new GridSettings
        {
            DefaultExportFormat = DefaultExportFormat,
            Settings = GridSettings
        };

        var value = JsonConvert.SerializeObject(userGridSettings);
        var response = await DataAccess.SetUserSetting(UserSettingsKeyConstants.AgencyBondGrid, value);

        if (!response.Success)
        {
            NotifySaveError(response.Errors, "user settings");
            return;
        }

        NotifySuccessfulSave("user settings");

        UserSettingsLoaded = true;
        UserSettingsChanged = false;
        StateHasChanged();
    }

    protected async Task GetGridSettings()
    {
        await ShowLoading();

        await DataAccess.GetCacheOrLoadDataAsync(UserSettingsLoad);
    }

    protected async Task ResetGridSettings()
    {
        await ShowLoading();

        GridSettings = null;
        DefaultExportFormat = ExportFormat.Excel;
        await Grid.ReloadSettings();

        var response = await DataAccess.ResetUserSetting(UserSettingsKeyConstants.AgencyBondGrid);
        if (!response.Success)
        {
            NotifySaveError(response.Errors, "user settings");
            return;
        }

        NotifySuccessfulSave("user settings");

        UserSettingsLoaded = false;
        UserSettingsChanged = false;
    }
}