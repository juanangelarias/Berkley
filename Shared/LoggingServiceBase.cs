using James.Shared.Model;
using Microsoft.Extensions.Logging;

namespace James.Shared;

public abstract class LoggingServiceBase : ILoggingService
{
    protected static readonly EventId MessageEventId = new(12341, "Log Message from Blazor client");
    protected static readonly EventId ExceptionEventId = new(12342, "Exception Logged from Blazor client");
    protected static readonly EventId PerformanceEventId = new(12343, "Performance Message from Blazor client");
    protected static readonly EventId UILoggingEventId = new(12344, "UI action from Blazor client");

    public abstract Task<bool> Log(EventId eventId, string message, string details, Severity severity, string category = "General",
        string? exceptionDetails = null, Dictionary<string, string>? data = null);

    public void LogVerbose(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
    {
        details = string.IsNullOrEmpty(details) ? message : details;
        Log(MessageEventId, message, details, Severity.Verbose, category, data: data);
    }

    public void LogDebug(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
    {
        details = string.IsNullOrEmpty(details) ? message : details;
        Log(MessageEventId, message, details, Severity.Debug, category, data: data);
    }

    public void LogInformation(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
    {
        details = string.IsNullOrEmpty(details) ? message : details;
        Log(MessageEventId, message, details, Severity.Information, category, data: data);
    }

    public void LogWarning(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
    {
        details = string.IsNullOrEmpty(details) ? message : details;
        Log(MessageEventId, message, details, Severity.Warning, category, data: data);
    }

    public void LogError(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
    {
        details = string.IsNullOrEmpty(details) ? message : details;
        Log(MessageEventId, message, details, Severity.Error, category, data: data);
    }

    public void LogException(Exception exception, string message, string details, Severity severity = Severity.Error, string category = "General",
        Dictionary<string, string>? data = null)
    {
        //TODO: Figure out a way to not log full details when not necessary.
        details = string.IsNullOrEmpty(details) ? "See exception details" : details;
        Log(severity >= Severity.Warning ? ExceptionEventId : MessageEventId, message, details, severity, category,
            exception.ToText(), data);
    }
}