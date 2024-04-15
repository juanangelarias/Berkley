using James.Shared;
using JamesWebUI.Client.GraphQL;
using Severity = James.Shared.Model.Severity;

namespace JamesWebUI.Client.Services
{
    public class LoggingService
    {
        private readonly JamesClient _jamesClient;
        public static readonly EventId MessageEventId = new(12341, "Log Message from Blazor client");
        public static readonly EventId ExceptionEventId = new(12342, "Exception Logged from Blazor client");
        public static readonly EventId PerformanceEventId = new(12343, "Performance Message from Blazor client");
        public static readonly EventId UILoggingEventId = new(12344, "UI action from Blazor client");

        public LoggingService(JamesClient jamesClient)
        {
            _jamesClient = jamesClient;
        }

        public async void Log(EventId eventId, string message, string details, Severity severity,
            string category = "General", string? exceptionMessage = null, string? exceptionDetails = null, 
            Dictionary<string, string>? data = null)
        {
//#pragma warning disable CS4014
//            await Task.Factory.StartNew(async () => 
//#pragma warning restore CS4014
//            {
                var graphQlData = ToGraphQlType(data);
                if (string.IsNullOrEmpty(exceptionMessage) && string.IsNullOrEmpty(exceptionDetails))
                {
                    var callData = new LogInformationInput
                    {
                        EventId = MessageEventId.Id,
                        Message = message,
                        Details = details,
                        Category = category,
                        //NOTE:  The below relies on the GraphQL type to be generated from the Model.Severity on the server
                        Severity = (GraphQL.Severity)Enum.Parse(typeof(GraphQL.Severity), Enum.GetName(severity)!, true),
                        Data = graphQlData
                    };
                    await _jamesClient.LogMessage.ExecuteAsync(callData);
                }
                else
                {
                    var callData = new LogExceptionInput
                    {
                        EventId = ExceptionEventId.Id,
                        Message = message,
                        Category = category,
                        //NOTE:  The below relies on the GraphQL type to be generated from the Model.Severity on the server
                        Severity = (GraphQL.Severity)Enum.Parse(typeof(GraphQL.Severity), Enum.GetName(severity)!, true),
                        Data = graphQlData,
                        Exception = exceptionMessage ?? "Exception thrown"
                    };
                    await _jamesClient.LogException.ExecuteAsync(callData);
                }
            //});
        }

        public void LogVerbose(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
        {
            details = string.IsNullOrEmpty(details) ? message : details;
            Log(MessageEventId, message, details, Severity.Verbose, category, data: data);
        }
        public void LogDebug(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
        {
            details = string.IsNullOrEmpty(details) ? message : details;
            Log(MessageEventId, message, details, Severity.Debug, category, data: data);
        }
        public void LogInformation(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
        {
            details = string.IsNullOrEmpty(details) ? message : details;
            Log(MessageEventId, message, details, Severity.Information, category, data: data);
        }
        public void LogWarning(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
        {
            details = string.IsNullOrEmpty(details) ? message : details;
            Log(MessageEventId, message, details, Severity.Warning, category, data: data);
        }
        public void LogError(string message, string details = "", string category = "General", Dictionary<string, string>? data = null)
        {
            details = string.IsNullOrEmpty(details) ? message : details;
            Log(MessageEventId, message, details, Severity.Error, category, data: data);
        }

        public void LogException(Exception exception, string message, string details, Severity severity = Severity.Error, string category = "General",
            Dictionary<string, string>? data = null)
        {
            //TODO: Figure out a way to not log full details when not necessary.
            details = string.IsNullOrEmpty(details) ? "See exception details" : details;
            Log(severity >= Severity.Warning ? ExceptionEventId : MessageEventId, message, details, severity, category,
                exception.Message, exception.ToText(), data);
        }

        static IReadOnlyList<KeyValuePairOfStringAndStringInput>? ToGraphQlType(Dictionary<string, string>? data)
        {
            if (data == null) return null;
            var ret = new List<KeyValuePairOfStringAndStringInput>();
            ret.AddRange(data.Select(kvp => new KeyValuePairOfStringAndStringInput { Key = kvp.Key, Value = kvp.Value }));
            return ret;
        }
    }
}
