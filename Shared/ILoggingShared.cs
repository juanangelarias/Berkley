using James.Shared.Model;

namespace James.Shared;

public interface ILoggingShared
{
    Task<bool> Log(int eventId, string message, string details, Severity severity,
        string category = "General", string? exceptionDetail = null,
        Dictionary<string, string>? data = null);

    public const string UserNameKeyString = "username";
}