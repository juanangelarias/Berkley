using System.Security.Claims;
using System.Text.RegularExpressions;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using ThemeService = JamesWebUI.Client.Services.ThemeService;

namespace JamesWebUI.Client.Shared;

public abstract class JamesLayoutComponentBase : LayoutComponentBase
{
    [Inject] public required ThemeService JamesThemeService { get; set; }
    [Inject] public required ILoggingService LoggingService { get; set; }
    [Inject] public required DialogService DialogService { get; set; }
    [Inject] public required NotificationService NotificationService { get; set; }

    #region Authentication Code

    [CascadingParameter] protected Task<AuthenticationState>? AuthenticationState { get; set; }
    private Task<AuthenticationState> LoadAuthenticationStateAsync { get; set; } = null!;

    //TODO: Discuss whether to allow only synchronous or asynchronous access to values.
    //Synchronous access properties
    protected AuthenticationState? State { get; private set; }
    protected ClaimsPrincipal? UserPrincipal => State?.User;
    protected bool IsAuthenticationStateLoaded { get; private set; }
    protected bool IsAuthenticated => IsAuthenticationStateLoaded && State!.User.Identity != null;

    //Asynchronous access to properties
    protected async ValueTask<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (LoadAuthenticationStateAsync.IsCompleted)
            return LoadAuthenticationStateAsync.Result;
        return await LoadAuthenticationStateAsync;
    }

    protected async ValueTask<ClaimsPrincipal> GetUserPrincipalAsync()
    {
        if (LoadAuthenticationStateAsync.IsCompleted)
            return LoadAuthenticationStateAsync.Result.User;
        return (await LoadAuthenticationStateAsync).User;
    }

    protected async ValueTask<bool> GetIsAuthenticatedAsync()
    {
        if (LoadAuthenticationStateAsync.IsCompleted)
            return null != LoadAuthenticationStateAsync.Result.User.Identity;
        return null != (await LoadAuthenticationStateAsync).User.Identity;
    }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (null != AuthenticationState)
            //NOTE:Running in background can cause issues if values are queried immediately upon return.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            {
                LoadAuthenticationStateAsync = AuthenticationState;
                State = await AuthenticationState;
                IsAuthenticationStateLoaded = true;
            });
    }

    #region Fields & Properties

    protected bool IsLoading;

    #endregion

    protected virtual async Task ShowLoading()
    {
        IsLoading = true;

        await Task.Yield();

        IsLoading = false;
    }

    protected virtual async Task ShowLoading(Task toExecute)
    {
        IsLoading = true;

        await toExecute;

        IsLoading = false;
    }

    #region Common Client Actions

    /// <summary>
    /// Generates standard notification that changes were saved successfully.
    /// </summary>
    /// <param name="itemSaved">The item being saved, default is "Changes".</param>
    protected void NotifySuccessfulSave(string itemSaved = "Changes")
    {
        itemSaved = itemSaved[..1].ToUpper() + itemSaved[1..];

        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Info,
            Summary = $"{itemSaved} saved successfully.",
            Duration = 5000
        });
    }

    /// <summary>
    /// Generates standard notification that a save failed.
    /// </summary>
    /// <param name="errors">Errors that were returned.</param>
    /// <param name="itemSaved">The item that didn't save, default is "changes".  Should NOT be title cased.</param>
    /// <param name="logError">If true will log the error using the LoggingService</param>
    protected void NotifySaveError(string[] errors, string itemSaved = "changes", bool logError = true)
    {
        var summary = $"There {(errors.Length == 1 ? "was an error" : "were errors")} " +
                      $"saving {itemSaved}.  {string.Join("  ", errors)}";

        if (logError)
            LoggingService.LogError(summary, errors);

        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Error,
            Summary = summary,
            Duration = 300000 // Treat as fatal 5 minutes
        });
    }

    /// <summary>
    /// Generates non-standard notification that a save event failed.
    /// </summary>
    /// <param name="errors">Errors that were returned.</param>
    /// <param name="notificationText">The item that didn't save, default is "changes".  Should NOT be title cased.</param>
    /// <param name="logError">If true will log the error using the LoggingService</param>
    /// <remarks>Use only when the standard message is not sufficient.  Text should be brief and details should be logged in the errors.</remarks>
    protected void NotifySaveIssue(string[] errors, string notificationText, bool logError = true)
    {
        if (logError)
            LoggingService.LogError(notificationText, errors);

        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Error,
            Summary = notificationText,
            Duration = 300000   // Treat as fatal 5 minutes
        });
    }

    /// <summary>
    /// Generates standard notification that a load failed.
    /// </summary>
    /// <param name="errors">Errors that were returned.</param>
    /// <param name="itemSaved">The item that didn't load, default is "data".  Should NOT be title cased.</param>
    /// <param name="fatal">True if no retries will happen, false if retries are continuing.</param>
    protected void NotifyLoadError(string[] errors, string itemSaved = "data", bool fatal = false)
    {
        var msg = fatal
            ? $"There {(errors.Length == 1 ? "was a fatal error" : "were fatal error(s)"
                )} retrieving {itemSaved}.  {string.Join("  ", errors)}"
            : $"There was an error retrieving {itemSaved}.  Retrying...";

        LoggingService.LogError(msg, errors);
        NotificationService.Notify(new NotificationMessage
        {
            Severity = fatal ? NotificationSeverity.Error : NotificationSeverity.Warning,
            Summary = msg,
            Duration = fatal ? 300000 : 15000 // Fatal: 5 minutes Else: 15 seconds
        });
    }

    protected void LogGraphQlLoadError(string[] errors, string message, string loadItem = "data", bool fatal = true)
    {
        if (fatal)
            LoggingService.LogError(message, category: StandardLoggingCategories.DataAccess,
                errors: errors, data: new()
                {
                    { "Failed load item", loadItem }, { "Source Class", GetType().Name }
                });
        else
            LoggingService.LogWarning(message, category: StandardLoggingCategories.DataAccess,
                errors: errors, data: new()
                {
                    { "Failed load item", loadItem }, { "Source Class", GetType().Name }
                });
    }

    /// <summary>
    /// Used to add UI notifications to LoadItems
    /// </summary>
    /// <param name="loadItem">Load Item to add events to</param>
    /// <param name="loadItemName">user-friendly name of data being load that will be used in notification if there are load issues.</param>
    /// <returns>Load event with events added</returns>
    /// <remarks>Typically used to surround load items when passing the do DataCache methods</remarks>
    protected LoadItem AddEventNotify(LoadItem loadItem, string loadItemName = "data")
    {
        loadItem.LoadError += LoadItemOnLoadError;

        void LoadItemOnLoadError(object? sender, LoadErrorEventArgs e)
        {
            NotifyLoadError(e.Errors, loadItemName, e.Fatal);
            LogGraphQlLoadError(e.Errors, $"Error loading {loadItemName}", loadItemName, e.Fatal);
        }

        return loadItem;
    }


    private static string SubstitutePropertyIfNeeded(string original, ExportColumnSubstitutions substitutions) =>
        string.IsNullOrWhiteSpace(original)
            ? original
            : (string.IsNullOrEmpty(substitutions[original].Property)
                ? original
                : substitutions[original].Property);

    private static string SubstituteFilterPropertyIfNeeded(string originalFilter,
        ExportColumnSubstitutions substitutions)
    {
        if (originalFilter == null!) return null!;
        foreach (var substitution in substitutions)
        {
            originalFilter = Regex.Replace(originalFilter, $"(?<=[(\\s\\(^]){substitution.Original}(?=[\\s\\)])",
                substitution.Property ?? "");
        }

        return originalFilter;
    }

    private string SubstituteTitleIfNeeded(string original, ExportColumnSubstitutions substitutions) =>
        string.IsNullOrWhiteSpace(original) ? original : substitutions[original].Title ?? "";

    public string ExportDataGridUrl<T>(RadzenDataGrid<T> dataGrid, string url, ExportFormat format,
        ExportColumnSubstitutions? propertySubstitutions = null)
    {
        propertySubstitutions ??= new();

        var selectColumns = dataGrid.ColumnsCollection
            .Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property))
            .Select(c => new
            {
                Property = SubstitutePropertyIfNeeded(c.Property, propertySubstitutions),
                Title = SubstituteTitleIfNeeded(c.Property, propertySubstitutions)
            }).ToList();
        selectColumns.AddRange(propertySubstitutions.Where(s => s.Value.Original == string.Empty)
            .Select(s => new { s.Value.Property, s.Value.Title }));
        var selectColumnString = string.Join(",",
            selectColumns
                .Select(cSub => cSub with
                {
                    Title = cSub.Title?.Replace(".", "_")
                })
                .Select(pt =>
                    pt.Property == pt.Title
                        ? pt.Property
                        : $"{pt.Property} as {pt.Title?.Replace(".", "_").Replace(' ', ExportColumnSubstitution.SpaceSubstitution)}"));
        var query = new Query()
        {
            OrderBy = SubstitutePropertyIfNeeded(dataGrid.Query.OrderBy, propertySubstitutions),
            Filter = SubstituteFilterPropertyIfNeeded(dataGrid.Query.Filter, propertySubstitutions),
            Select = selectColumnString
        };
        return query.ToUrl($"{url}/{(format == ExportFormat.CSV ? "CSV" : "Excel")}");
    }

    protected async Task HandleError(string defaultError, string[] errors)
    {
        var errorMessage = errors.Length == 0
            ? [defaultError]
            : errors;

        await InvokeAsync(() => NotifyLoadError(errorMessage));
    }

    #endregion
}

//TODO: Review if this is still needed after permissioning is fleshed out
public static class AuthUserExtensions
{
    public static string? Username(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;
    }

    public static string? EmailAddress(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "email_address")?.Value;
    }

    public static string? FirstName(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
    }

    public static string? LastName(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
    }

    public static string? PictureUrl(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
    }

    public static string? UpdatedAt(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "updated_at")?.Value;
    }

    public static string? NameIdentifier(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "email_address")?.Value;
    }

    public static string? SID(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
    }
}