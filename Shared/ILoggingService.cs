using James.Shared.Model;
using Microsoft.Extensions.Logging;

namespace James.Shared;

public interface ILoggingService
{
    Task<bool> Log(EventId eventId, string message, string details, Severity severity,
        string category = "General", string? exceptionDetails = null, 
        Dictionary<string, string>? data = null);

    void LogVerbose(string message, string details = "", string category = "General", Dictionary<string, string>? data = null);
    void LogDebug(string message, string details = "", string category = "General", Dictionary<string, string>? data = null);
    void LogInformation(string message, string details = "", string category = "General", Dictionary<string, string>? data = null);
    void LogWarning(string message, string details = "", string category = "General", Dictionary<string, string>? data = null);
    void LogError(string message, string details = "", string category = "General", Dictionary<string, string>? data = null);

    void LogException(Exception exception, string message, string details, Severity severity = Severity.Error, string category = "General",
        Dictionary<string, string>? data = null);
}