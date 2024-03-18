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
                if (input is LogExceptionInput lei)
                    //TODO:  Keep list of event ids?
                    _logger.Log(LogLevel.Information, new EventId(input.EventId), input.Message, new Exception(message:lei.ExceptionMessage));
                else
                    _logger.Log(LogLevel.Information, new EventId(input.EventId), input.Message);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(12345, "LoggingFailure"), e, "Exception thrown while logging");
                return false;
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
        public Severity Severity { get; set; } = Severity.Error;
    }

    public class LogExceptionInput : LogInput
    {
        //public Exception Exception { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionDetail { get; set; }
    }
}
