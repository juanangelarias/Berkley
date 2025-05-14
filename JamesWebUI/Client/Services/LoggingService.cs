using System.Diagnostics;
using James.Shared;
using James.Data.Client.GraphQL;
using Severity = James.Shared.Model.Severity;

namespace JamesWebUI.Client.Services
{
    public class LoggingService(JamesClient jamesClient) : LoggingServiceBase
    {
        public override async Task<bool> Log(EventId eventId, string message, string details, Severity severity,
            string category = "General", string? exceptionDetails = null, 
            Dictionary<string, string>? data = null)
        {
//#pragma warning disable CS4014
//            await Task.Factory.StartNew(async () => 
//#pragma warning restore CS4014
//            {
            try
            {
                var graphQlData = ToGraphQlType(data);
                if (string.IsNullOrEmpty(exceptionDetails))
                {
                    var callData = new LogInformationInput
                    {
                        EventId = MessageEventId.Id,
                        Message = message,
                        Details = details,
                        Category = category,
                        //NOTE:  The below relies on the GraphQL type to be generated from the Model.Severity on the server
                        Severity = (James.Data.Client.GraphQL.Severity)Enum.Parse(typeof(Severity), Enum.GetName(severity)!, true),
                        Data = graphQlData
                    };
                    await jamesClient.LogMessage.ExecuteAsync(callData);
                }
                else
                {
                    var callData = new LogExceptionInput
                    {
                        EventId = ExceptionEventId.Id,
                        Message = message,
                        Category = category,
                        //NOTE:  The below relies on the GraphQL type to be generated from the Model.Severity on the server
                        Severity = (James.Data.Client.GraphQL.Severity)Enum.Parse(typeof(Severity), Enum.GetName(severity)!, true),
                        Data = graphQlData,
                        Exception = exceptionDetails
                    };
                    await jamesClient.LogException.ExecuteAsync(callData);
                }
                return true;
            }
            catch (Exception ex)
            {
                //Nothing much can be done to log the error if the exception is caused by trying to log the previous exception.
                Debug.WriteLine(ex);
                return false;
            }
            //});
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
