using HotChocolate.Authorization;
using James.Shared.Model;
using JamesWebUI.Client.Services;
using JamesWebUI.Server.SharedServices;

namespace JamesWebUI.Server.GraphQL.Mutations
{
    [MutationType]
    //public class LoggingMutationType:ObjectType<LoggingMutation>{}
    public class LoggingMutation
    {
        private readonly ILogger<LoggingMutation> _logger;
        private readonly IUserShared _userShared;

        public LoggingMutation(ILogger<LoggingMutation> logger, IUserShared userShared)
        {
            _logger = logger;
            _userShared = userShared;
        }

        public async Task<bool> LogInformation(int eventId, string message, string details, Severity severity,
            string category = "General",
            Dictionary<string, string>? data = null)
        {
            return await Log(eventId, message, details, severity, category, null, data);
        }

        private async Task<bool> Log(int eventId, string message, string details, Severity severity,
                string category = "General", string? exceptionDetail = null,
                Dictionary<string, string>? data = null)
        {
            try
            {
                var username = (await _userShared.GetCurrentUser()).Username;
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
                        foreach (var datum in data)
                            extraInfo.Add(datum.Key, datum.Value);
                    using (_logger.BeginScope(extraInfo))
                        _logger.Log(GetLogLevel(severity), eventId, message: message);
                });
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(12345, "LoggingFailure"), e, "Exception thrown while logging");
                return false;
            }
        }

        public async Task<bool> LogException(int eventId, string message, string exception, Severity severity,
            string category = "General",
            Dictionary<string, string>? data = null)
        {
            return await Log(eventId, message, exception, severity, exception, category, data);
        }

        //TODO:Get rid of this after testing
        [Authorize]
        public async Task<string> LogUser(string message = "")
        {
            var userInfo = await _userShared.GetCurrentUser();
            await LogInformation(LoggingService.MessageEventId.Id, string.IsNullOrWhiteSpace(message) ? "Logging authenticated user" : "Logging authenticated user with message " + message,
                userInfo.Username ?? "", Severity.Information);
            return userInfo.Username ?? "Unknown user" + (string.IsNullOrWhiteSpace(message) ? "" : " " + message);
        }

        private static LogLevel GetLogLevel(Severity severity)
        {
            switch (severity)
            {
                case Severity.Verbose:
                    return LogLevel.Trace;
                case Severity.Debug:
                    return LogLevel.Debug;
                case Severity.Information:
                    return LogLevel.Information;
                case Severity.Warning:
                    return LogLevel.Warning;
                case Severity.Error:
                    return LogLevel.Error;
                case Severity.Fatal:
                    return LogLevel.Critical;
                default:
                    return LogLevel.None;
            }
        }
    }
}
