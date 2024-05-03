using James.Shared.Model;
using JamesWebUI.Server.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace JamesWebUI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IHttpClientFactory _httpClientFactory;
        private static HttpClient? _httpClient;
        private  ILogger<UserController> _logger;

        private HttpClient UserInfoClient => _httpClient ??= _httpClientFactory.CreateClient("Auth0UserInfo");

        public UserController(IHttpClientFactory httpClientFactory, ILogger<UserController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient ??= httpClientFactory.CreateClient("Auth0UserInfo");
            _logger = logger;
            _cachedAuth0 ??= new UserInformationCache<IAuth0UserInfo> { LookupTask = GetAuth0UserInfo };
        }

        private static UserInformationCache<IAuth0UserInfo> _cachedAuth0 = null!;
        private static readonly JwtSecurityTokenHandler _handler = new();

        [HttpGet("/GetCurrentUserInfo")]
        public JsonResult GetCurrentUserInfo()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
                var userInfo = GetUserInfoAsync(token).Result;
                Debug.WriteLine($"User info returned: {userInfo}");
                return new JsonResult(userInfo);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new JsonResult(e.ToString()) { StatusCode = 500 };
            }
        }

        public async Task<IAuth0UserInfo> GetAuth0UserInfo(string token)
        {
            try
            {
                var jwtSecurityToken = _handler.ReadJwtToken(token);
                var userInfoEndpoint = jwtSecurityToken.Audiences.First(a => a.StartsWith("http"));
                if (UserInfoClient.DefaultRequestHeaders.Authorization != null)
                    UserInfoClient.DefaultRequestHeaders.Remove("Authorization");
                var reqHeaderAuthorization = "bearer " + token;//Request.Headers[HeaderNames.Authorization].ToString();
                UserInfoClient.DefaultRequestHeaders.Add("Authorization", reqHeaderAuthorization);
                var userInfo = await UserInfoClient.GetFromJsonAsync<Auth0UserInfo>(userInfoEndpoint);
                Debug.WriteLine("Returning Auth0 data");
                return userInfo!;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(13321, "GetUserInfoError"), ex, "Exception thrown while getting Auth0 user information.");
                throw;
            }
        }

        public async Task<SiteUserInfo> GetUserInfoAsync(string jwt)
        {
            _logger.LogDebug("UserInfo request received.");
            
            //Get Email from JWT
            JwtSecurityToken jwtSecurityToken;
            SiteUserInfo siteUserInfo;
            try
            {
                jwtSecurityToken = _handler.ReadJwtToken(jwt);
                //var userInfoEndpoint = jwtSecurityToken.Audiences.First(a => a.StartsWith("http"));

                siteUserInfo = new SiteUserInfo
                {
                    JWT = jwt,
                    Email = jwtSecurityToken.Claims.FirstOrDefault(c =>
                        c.Type == "http://schemas.wrberkley.com/identity/application/claims/email")?.Value ?? ""
                };
                //Call Auth0's 
                if (UserInfoClient.DefaultRequestHeaders.Authorization != null)
                    UserInfoClient.DefaultRequestHeaders.Remove("Authorization");
                UserInfoClient.DefaultRequestHeaders.Add("Authorization", "bearer " + jwt);
                Debug.Assert(_cachedAuth0 != null, nameof(_cachedAuth0) + " != null");
                var auth0Info = await _cachedAuth0.GetAsync(jwt);

                //TODO: Do error handling
                siteUserInfo.FirstName = auth0Info?.FirstName;
                siteUserInfo.LastName = auth0Info?.LastName;
                siteUserInfo.FullName = auth0Info?.FullName;
                siteUserInfo.Username = auth0Info?.Username;
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(13321, "GetUserInfoError"), e,
                    "Exception thrown while getting user information.");
                throw;
            }

            return siteUserInfo;
        }
    }
}
