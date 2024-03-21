using System.Runtime.CompilerServices;
using James.Shared.Model;

namespace JamesWebUI.Client.Services
{
    public class LoggingService
    {
        private readonly ILogger _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public async Task Log(EventId eventId, string message, string details, Severity severity,
            string category = "General", string? ExceptionMessage = null, string? ExceptionDetails = null)
        {
//#pragma warning disable CS4014
//            Task.Factory.StartNew(() =>
//#pragma warning restore CS4014
//            {
//                var extraInfo = new Dictionary<string, object>()
//                {
//                    ["Category"] = category,
//                    ["Details"] = details
//                };
//                if (ExceptionMessage != null)
//                    extraInfo.Add(nameof(ExceptionMessage), ExceptionMessage);
//                if (ExceptionDetails != null)
//                    extraInfo.Add(nameof(ExceptionDetails), ExceptionDetails);
//                using (_logger.BeginScope(extraInfo))
//                    _logger.Log(GetLogLevel(severity), eventId, message: message);
//            });
        }
    }
}
