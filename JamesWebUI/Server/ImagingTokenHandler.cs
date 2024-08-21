using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using James.Shared.Kong0;
using James.Shared.Server.Kong0;
using Microsoft.AspNetCore.Authentication;

namespace JamesWebUI.Server
{
    public class ImagingTokenHandler(IHttpClientFactory httpClientFactory) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var accessToken = await GetImagingToken();
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }

        //Cache token for all users
        private static KongToken? _cachedToken;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
            WriteIndented = true
        };

        internal static KongTokenRequest ImagingCredentials { get; set; }

        private async Task<KongToken> GetImagingToken()
        {
            //TODO: Build in resiliency for when Kong0 service is down.
            if (null == _cachedToken || _cachedToken.IsExpired)
            {
                var client = httpClientFactory.CreateClient("P8FileNetTokens");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var tokenRequestContent = new StringContent(JsonSerializer.Serialize(ImagingCredentials, _jsonOptions));
                var response = client.PostAsync(client.BaseAddress!.ToString(), tokenRequestContent); 
                //var response = client.PostAsJsonAsync("/", ImagingCredentials, _jsonOptions);
                if (null != response.Exception && response.Exception.InnerExceptions.Any())
                    throw response.Exception;
                _cachedToken = await
                    JsonSerializer.DeserializeAsync<KongToken>(await response.Result.Content.ReadAsStreamAsync(),
                        _jsonOptions);
                _cachedToken.Received = response.Result.Headers.Date!.Value.DateTime;
            }

            return _cachedToken;
        }

    }
}
