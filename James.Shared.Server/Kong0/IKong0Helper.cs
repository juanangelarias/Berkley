using James.Shared.Kong0;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.Json;

namespace James.Shared.Server.Kong0
{
    public interface IKong0Helper
    {
        //public void SetKong0Credentials(KongTokenRequest request, string httpClientName);
        public Task<T?> ExecuteMethodAsync<T, TClientInterface>(ClientBase<TClientInterface> client, Func<ClientBase<TClientInterface>, Task<T>> clientFunc) where TClientInterface : class;
    }
    public abstract class Kong0HelperBase(IHttpClientFactory httpClientFactory, IKongCredentialCache credentialCache, ILoggingService loggingService) : IKong0Helper
    {
        protected abstract string HttpClientName { get; }

        protected abstract CustomBinding? CustomBinding { get; }

        public static bool DetailedLogging { get; set; }

        public async Task<T?> ExecuteMethodAsync<T, TClientInterface>(ClientBase<TClientInterface> client, Func<ClientBase<TClientInterface>, Task<T>> clientFunc) where TClientInterface : class
        {
            var token = await GetTokenAsync();
            if (token == null)
            {
                loggingService.LogError("Kong0 Token could not be attained.", "See previous error.", "Kong0");
                return default;
            }
            if (null != CustomBinding)
                client.Endpoint.Binding = CustomBinding;

            using (var _ = new OperationContextScope(client.InnerChannel))
            {
                var requestProperty = new HttpRequestMessageProperty
                {
                    Headers =
                    {
                        [HttpRequestHeader.Authorization] = $"{token.TokenType} {token.AccessToken}"
                    }
                };
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestProperty;
                return await clientFunc(client);
            }
        }
        //Cache token for all users
        protected async Task<KongToken?> GetTokenAsync()
        {
            //TODO: Build in resiliency for when Kong0 service is down.
            var cachedToken = credentialCache.TokenCache.Keys.Contains(GetType()) ? credentialCache.TokenCache[GetType()] : null;
            if (null == cachedToken || cachedToken.IsExpired)
            {
                var tokenRetrievalClient = httpClientFactory.CreateClient(HttpClientName);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var tokenRequestCredentials = KongTokenRequest.GetRequest(GetType());
                var requestPayload = JsonSerializer.Serialize(tokenRequestCredentials, JsonOptions);
                if (DetailedLogging)
                    loggingService.LogDebug("Kong0 Token Request Detail", requestPayload, "Kong0");
                var myTokenContent = new StringContent(requestPayload, Encoding.UTF8, "application/json");
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenRetrievalClient.BaseAddress);
                tokenRequest.Content = myTokenContent;
                var response = await tokenRetrievalClient.PostAsync(tokenRetrievalClient.BaseAddress, myTokenContent);
                if (response.IsSuccessStatusCode == false)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    loggingService.LogError("Error retrieving token from Kong0", content, "Kong0", data: new Dictionary<string, string> { { "Status Code", response.StatusCode.ToString() } });
                    throw new Exception("Failed to get Kong0 token for P8 Filenet");
                }
                StreamReader? sr = null;
                try
                {
                    var resultContentStream = await response.Content.ReadAsStreamAsync();
                    if (DetailedLogging)
                    {
                        //Slows performance.  Only execute if detailed logs are requested
                        sr = new StreamReader(resultContentStream);
                        var resultText = await sr.ReadToEndAsync();
                        loggingService.LogDebug("Kong0 Token Result Detail", resultText, "Kong0");
                        resultContentStream.Position = 0;
                    }

                    credentialCache.TokenCache[GetType()] = (await
                        JsonSerializer.DeserializeAsync<KongToken>(resultContentStream, JsonOptions))!;
                    credentialCache.TokenCache[GetType()].Received = response.Headers.Date!.Value.DateTime.ToLocalTime();
                }
                catch (Exception exception)
                {
                    //TODO:Handle errors
                    loggingService.LogException(exception, "Error reading response from Kong0");
                    return null;
                }
                finally
                {
                    //Cannot dispose earlier or it will close the underlying stream
                    sr?.Dispose();
                }
            }
            return credentialCache.TokenCache[GetType()];
        }

        protected static JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true
        };
    }

    public interface IKongCredentialCache
    {
        public Dictionary<Type, KongToken> TokenCache { get; }
    }

    /// <summary>
    /// Credential Cache that can be instantiated with dependency injection.
    /// </summary>
    /// <remarks>Should always be scoped as a singleton</remarks>
    public class KongCredentialCache : IKongCredentialCache
    {
        public Dictionary<Type, KongToken> TokenCache { get; } = new();
    }
}
