using James.Shared;
using James.Shared.Data;
using Radzen;

namespace JamesWebUI.Client.States;

public interface IStateBase
{
    public void LogGraphQlLoadError(string[] errors, string message, string loadItem = "data", bool fatal = true);
    public LoadItem AddEventNotify(LoadItem loadItem, string loadItemName = "data");
    public void NotifyLoadError(string[] errors, string itemSaved = "data", bool fatal = false);
}

public class StateBase(ILoggingService loggingService,
    NotificationService notificationService): NotifyPropertyChanged, IStateBase
{
    private readonly ILoggingService _loggingService = loggingService;
    private readonly NotificationService _notificationService = notificationService;
    
    public void LogGraphQlLoadError(string[] errors, string message, string loadItem = "data", bool fatal = true)
    {
        if (fatal)
            _loggingService.LogError(message, category: StandardLoggingCategories.DataAccess,
                errors: errors, data: new()
                {
                    { "Failed load item", loadItem }, { "Source Class", GetType().Name }
                });
        else
            _loggingService.LogWarning(message, category: StandardLoggingCategories.DataAccess,
                errors: errors, data: new()
                {
                    { "Failed load item", loadItem }, { "Source Class", GetType().Name }
                });
    }

    public LoadItem AddEventNotify(LoadItem loadItem, string loadItemName = "data")
    {
        loadItem.LoadError += LoadItemOnLoadError;

        void LoadItemOnLoadError(object? sender, LoadErrorEventArgs e)
        {
            NotifyLoadError(e.Errors, loadItemName, e.Fatal);
            LogGraphQlLoadError(e.Errors, $"Error loading {loadItemName}", loadItemName, e.Fatal);
        }

        return loadItem;
    }
    
    public void NotifyLoadError(string[] errors, string itemSaved = "data", bool fatal = false)
    {
        var msg = fatal
            ? $"There {(errors.Length == 1 ? "was a fatal error" : "were fatal error(s)"
                )} retrieving {itemSaved}.  {string.Join("  ", errors)}"
            : $"There was an error retrieving {itemSaved}.  Retrying...";

        _loggingService.LogError(msg, errors);
        _notificationService.Notify(new()
        {
            Severity = fatal ? NotificationSeverity.Error : NotificationSeverity.Warning,
            Summary = msg,
            Duration = fatal ? 300000 : 15000 // Fatal: 5 minutes Else: 15 seconds
        });
    }
}