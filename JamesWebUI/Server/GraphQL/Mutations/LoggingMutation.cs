using James.Shared.Model;

namespace JamesWebUI.Server.GraphQL.Mutations
{
    [MutationType]
    //public class LoggingMutationType:ObjectType<LoggingMutation>{}
    public class LoggingMutation
    {
        private readonly ILogger<LoggingMutation> _logger;

        public LoggingMutation(ILogger<LoggingMutation> logger)
        {
            _logger = logger;
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

                //TODO:  Figure out if  User can be gotten and included in log
                //if (input is LogExceptionInput lei)
                //    //TODO:  Keep list of event ids?
                //    _logger.Log(LogLevel.Information, new EventId(input.EventId), input.Message, new Exception(message:lei.ExceptionMessage));
                //else
                //    _logger.Log(LogLevel.Information, new EventId(input.EventId), input.Message);
#pragma warning disable CS4014
                await Task.Factory.StartNew(() =>
#pragma warning restore CS4014
                {
                    var extraInfo = new Dictionary<string, object>()
                    {
                        ["Category"] = category,
                        ["Details"] = details
                    };
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
            return await Log(eventId, message, exception, severity,exception, category, data);
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
