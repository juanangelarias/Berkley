using James.Shared.Model;
using JamesWebUI.Client.Model;
using JamesWebUI.Client.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JamesWebUI.Client.Shared;

/// <summary>
/// Represents the base class for a grid layout component within the JamesWebUI application.
/// Provides functionality for managing data grid settings, user preferences, and configurations.
/// </summary>
/// <typeparam name="T">
/// The type of the data model displayed in the grid. Must be a class.
/// </typeparam>
/// <remarks>
/// This abstract class extends the <see cref="JamesLayoutComponentBase"/> and provides reusable
/// functionality for handling grid-specific operations such as loading, saving, and resetting user settings,
/// as well as setting default export formats for data operations.
/// </remarks>
///

// HACK: This is valid for a component with a SINGLE grid. If more than one grid is needed, then a component
// for each grid should be created
public abstract class JamesGridLayoutComponentBase<T> : JamesLayoutComponentBase
    where T : class
{
    [Inject]
    public IUserSettingService UserSettingService { get; set; } = null!;

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

    protected string UserSettingsKey { get; set; } = string.Empty;
    protected RadzenDataGrid<T> Grid { get; set; } = null!;
    
    #endregion

    protected async Task SaveGridSettings()
    {
        await ShowLoading();

        var userGridSettings = new GridSettings
        {
            DefaultExportFormat = DefaultExportFormat,
            Settings = GridSettings
        };

        var response = await UserSettingService.SetGridSettings(userGridSettings);
        
        if (!response.Success)
        {
            NotifySaveError(response.Errors, "user grid settings");
            return;
        }

        NotifySuccessfulSave("user grid settings");

        UserSettingsLoaded = true;
        UserSettingsChanged = false;
        StateHasChanged();
    }

    protected async Task GetGridSettings()
    {
        await ShowLoading();
        var response = await UserSettingService.GetGridSettings();
        if (response == null)
            return;

        DefaultExportFormat = response.DefaultExportFormat;
        GridSettings = response.Settings;
        UserSettingsLoaded = true;
        UserSettingsChanged = false;
    }

    protected async Task ClearGridSettings()
    {
        await ShowLoading();

        GridSettings = null;
        DefaultExportFormat = ExportFormat.Excel;
        await Grid.ReloadSettings();

        var response = await UserSettingService.ResetUserSettings();
        if (!response.Success)
        {
            NotifySaveError(response.Errors, "user grid settings");
            return;
        }

        NotifySuccessfulSave("user grid settings");

        UserSettingsLoaded = false;
        UserSettingsChanged = false;
    }
}