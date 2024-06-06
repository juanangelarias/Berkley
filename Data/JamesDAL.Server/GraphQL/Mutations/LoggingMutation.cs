using James.Shared.Server;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    //public class LoggingMutationType:ObjectType<LoggingMutation>{}
    public class LoggingMutation
    {
        private readonly ILoggingShared _logger;
        private readonly IUserShared _userShared;

        public LoggingMutation(ILoggingShared logger, IUserShared userShared)
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
            return await _logger.Log(eventId, message, details, severity, category, exceptionDetail, data);
        }

        public async Task<bool> LogException(int eventId, string message, string exception, Severity severity,
            string category = "General",
            Dictionary<string, string>? data = null)
        {
            return await Log(eventId, message, exception, severity, exception, category, data);
        }

        //TODO:Get rid of this after testing
        //[Authorize]
        public async Task<string> LogUser(string message = "")
        {
            var userInfo = await _userShared.GetCurrentUser();
            await LogInformation(LoggingServiceBase.MessageEventId.Id, string.IsNullOrWhiteSpace(message) ? "Logging authenticated user" : "Logging authenticated user with message " + message,
                userInfo.Username ?? "", Severity.Information);
            return userInfo.Username ?? "Unknown user" + (string.IsNullOrWhiteSpace(message) ? "" : " " + message);
        }
    }
}
