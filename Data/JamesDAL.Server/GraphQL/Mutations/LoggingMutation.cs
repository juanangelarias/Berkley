using James.Shared.Server;
using System.Security.Claims;
using James.Shared;

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
            ClaimsPrincipal claimsPrincipal, string category = "General",
            Dictionary<string, string>? data = null)
        {
            //Add username if it is included
            var username= claimsPrincipal.FindFirstValue("nickname");
            if (null != username)
            {
                data ??= new Dictionary<string, string>();
                //If the username was sent in, but the username sent in does not match the claims principal, save as "sentInUsername"
                var sentInUserName = data.ContainsKey("username")?data[ILoggingShared.UserNameKeyString]:null;
                if (null != sentInUserName && sentInUserName != username)
                    data["sentInUsername"] = data["username"];
                data[ILoggingShared.UserNameKeyString] = username;
            }
            return await Log(eventId, message, details, severity, category, null, data);
        }

        private async Task<bool> Log(int eventId, string message, string details, Severity severity,
                string category = "General", string? exceptionDetail = null,
                Dictionary<string, string>? data = null)
        {
            return await _logger.Log(eventId, message, details, severity, category, exceptionDetail, data);
        }

        public async Task<bool> LogException(int eventId, string message, string exception, Severity severity,
            ClaimsPrincipal claimsPrincipal, string category = "General",
            Dictionary<string, string>? data = null)
        {
            //Add username if it is included
            var username= claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (null != username)
            {
                data ??= new Dictionary<string, string>();
                //If the username was sent in, but the username sent in does not match the claims principal, save as "sentInUsername"
                var sentInUserName = data.ContainsKey("username")?data[ILoggingShared.UserNameKeyString]:null;
                if (null != sentInUserName && sentInUserName != username)
                    data["sentInUsername"] = data["username"];
                data[ILoggingShared.UserNameKeyString] = username;
            }
            return await Log(eventId, message, exception, severity, exception, category, data);
        }

        //TODO:Get rid of this after testing
        //[Authorize]
        public async Task<string> LogUser(ClaimsPrincipal claimsPrincipal, string message = "")
        {
            var userInfo = await _userShared.GetCurrentUser();
            await LogInformation(LoggingServiceBase.MessageEventId.Id, string.IsNullOrWhiteSpace(message) ? "Logging authenticated user" : "Logging authenticated user with message " + message,
                userInfo.Username ?? "", Severity.Information, claimsPrincipal);
            return userInfo.Username ?? "Unknown user" + (string.IsNullOrWhiteSpace(message) ? "" : " " + message);
        }
    }
}
