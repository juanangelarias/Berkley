using James.Shared.Model;
using Microsoft.Extensions.Logging;

namespace James.Shared;

public abstract class LoggingServiceBase : ILoggingService
{
    public static readonly EventId MessageEventId = new(12341, "Log Message from Blazor client");
    public static readonly EventId ExceptionEventId = new(12342, "Exception Logged from Blazor client");
    public static readonly EventId PerformanceEventId = new(12343, "Performance Message from Blazor client");
    public static readonly EventId UILoggingEventId = new(12344, "UI action from Blazor client");

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

    /// <summary>
    /// Logs errors
    /// </summary>
    /// <remarks>Designed for efficient logging of IDataAccess errors without excessive string concatenation</remarks>
    /// <param name="message">Error message (Identifies process that had error)</param>
    /// <param name="errors">Array of errors</param>
    /// <param name="category">Category.  Use StandardLoggingCategories values when possible.</param>
    /// <param name="data">Data relevant to the error.</param>
    public void LogError(string message, string[] errors, string category = "General", Dictionary<string, string>? data = null)
    {
        data ??= new Dictionary<string, string>();
        if (errors.Length==1&& !data.ContainsKey("Error"))
            data.Add("Error", errors[0]);
        else
        {
            var errorNumOffset = 0;
            for(var i =1;i<=errors.Length;i++)
            {
                var key = "Error" + (i+ errorNumOffset);
                while (data.ContainsKey(key))
                    key = "Error" + ++errorNumOffset;
                data.Add(key, errors[i-1]);
            }
        }
        Log(MessageEventId, message, "See data for details", Severity.Error, category, data: data);
    }

    public void LogException(Exception exception, string message, string details="", Severity severity = Severity.Error, string category = "General",
        Dictionary<string, string>? data = null)
    {
        details = string.IsNullOrEmpty(details) ? "See exception details" : details;
        Log(severity >= Severity.Warning ? ExceptionEventId : MessageEventId, message, details, severity, category,
            exception.ToText(), data);
    }
}