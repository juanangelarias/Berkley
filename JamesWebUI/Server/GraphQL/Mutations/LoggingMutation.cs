using James.Shared.Model;
using Microsoft.Data.SqlClient;

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
        public async Task<bool> LogInformation(LogInput input)
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
                Task.Factory.StartNew(() =>
#pragma warning restore CS4014
                {
                    var extraInfo = new Dictionary<string, object>()
                    {
                        ["Category"] = input.Category??"General",
                        ["Details"] = input.Details
                    };
                    if (input is LogExceptionInput lei)
                    {
                        if (lei.ExceptionMessage != null)
                            extraInfo.Add(nameof(lei.ExceptionMessage), lei.ExceptionMessage);
                        if (lei.ExceptionDetail != null)
                            extraInfo.Add(nameof(lei.ExceptionDetail), lei.ExceptionDetail);
                    }
                    using (_logger.BeginScope(extraInfo))
                        _logger.Log(GetLogLevel(input.Severity), input.EventId, message: input.Message);
                });
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(12345, "LoggingFailure"), e, "Exception thrown while logging");
                return false;
            }
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

        public async Task<bool> LogException(LogExceptionInput input)
        {
            return await LogInformation(input);
        }
    }

    public class LogInput
    {
        public int EventId { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string? Category { get; set; }
        public Severity Severity { get; set; } = Severity.Error;
    }

    public class LogExceptionInput : LogInput
    {
        public string ExceptionMessage { get; set; }
        public string ExceptionDetail { get; set; }
    }
}
