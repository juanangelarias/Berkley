using James.Shared.Model;
using Microsoft.Extensions.Logging;

namespace James.Shared.Server
{
    public class ServerLoggingService:LoggingServiceBase
    {
        private readonly ILoggingShared _logger;

        public ServerLoggingService(ILoggingShared logger)
        {
            _logger = logger;
        }
        public override async Task<bool> Log(EventId eventId, string message, string details, Severity severity, string category = "General",
            string? exceptionDetail = null, Dictionary<string, string>? data = null)
        {
            return await _logger.Log(eventId.Id, message, details, severity, category, exceptionDetail, data);
        }
    }
}
