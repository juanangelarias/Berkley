using James.Shared.Model;
using James.Shared.Server;

namespace JamesWebUI.Server.SharedServices
{
    public class LoggingShared(ILogger<LoggingShared> logger, IUserShared userShared) : ILoggingShared
    {
        public async Task<bool> Log(int eventId, string message, string details, Severity severity,
                string category = "General", string? exceptionDetail = null,
                Dictionary<string, string>? data = null)
        {
            try
            {
                var username = (data?.ContainsKey("username") == true)? data["username"]:(await userShared.GetCurrentUser()).Username ;
#pragma warning disable CS4014
                await Task.Factory.StartNew(() =>
#pragma warning restore CS4014
                {
                    var extraInfo = new Dictionary<string, object>()
                    {
                        ["Category"] = category,
                        ["Details"] = details
                    };
                    if (null != username) extraInfo.Add("User", username);
                    if (!string.IsNullOrEmpty(exceptionDetail))
                        extraInfo.Add(nameof(exceptionDetail), exceptionDetail);
                    if (data != null)
                        foreach (var datum in data.Where(d=>d.Key!="username"))
                            extraInfo.Add(datum.Key, datum.Value);
                    using (logger.BeginScope(extraInfo))
                        logger.Log(GetLogLevel(severity), eventId, message: message);
                });
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(new EventId(12345, "LoggingFailure"), e, "Exception thrown while logging");
                return false;
            }
        }

        private static LogLevel GetLogLevel(Severity severity)
        {
            return severity switch
            {
                Severity.Verbose => LogLevel.Trace,
                Severity.Debug => LogLevel.Debug,
                Severity.Information => LogLevel.Information,
                Severity.Warning => LogLevel.Warning,
                Severity.Error => LogLevel.Error,
                Severity.Fatal => LogLevel.Critical,
                _ => LogLevel.None
            };
        }
    }
}
