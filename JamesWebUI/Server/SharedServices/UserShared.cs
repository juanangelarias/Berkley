using James.Shared.Model;
using JamesWebUI.Server.Controllers;
using JamesWebUI.Server.Model;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace JamesWebUI.Server.SharedServices
{
    public class UserShared : IUserShared
    {
        private IHttpClientFactory _httpClientFactory;
        private static HttpClient? _httpClient;
        private  ILogger<UserController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private HttpClient UserInfoClient => _httpClient ??= _httpClientFactory.CreateClient("Auth0UserInfo");

        public UserShared(IHttpClientFactory httpClientFactory, ILogger<UserController> logger, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient ??= httpClientFactory.CreateClient("Auth0UserInfo");
            _logger = logger;
            _httpContextAccessor = contextAccessor;
            // ReSharper disable once ConstantNullCoalescingCondition
            _cachedAuth0 ??= new UserInformationCache<IAuth0UserInfo>(lookupTask: GetAuth0UserInfo);
            //_cachedSiteUserInfo ??= new UserInformationCache<SiteUserInfo>{LookupTask = }
        }

        private static UserInformationCache<IAuth0UserInfo> _cachedAuth0 = null!;
        //private static UserInformationCache<SiteUserInfo> _cachedSiteUserInfo = null!;
        private static readonly JwtSecurityTokenHandler _handler = new();

        public async Task<SiteUserInfo> GetCurrentUser()
        {
            Debug.Assert(null != _httpContextAccessor.HttpContext, "Must be called with a non-null HttpContext");
            var jwt = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
            var userInfo = await GetUserInfoAsync(jwt);
            return userInfo;
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
                siteUserInfo = new SiteUserInfo
                {
                    JWT = jwt,
                    Email = jwtSecurityToken.Claims.FirstOrDefault(c =>
                        c.Type == "http://schemas.wrberkley.com/identity/application/claims/email")?.Value ?? ""
                };

                Debug.Assert(_cachedAuth0 != null, nameof(_cachedAuth0) + " != null");
                //NOTE:  If user data needs to be loaded from other sources, do it here
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

    public interface IUserShared
    {
        //public Task<IAuth0UserInfo> GetAuth0UserInfo(string jwtToken);
        public Task<SiteUserInfo> GetCurrentUser();
        public Task<SiteUserInfo> GetUserInfoAsync(string jwtToken);
    }
}
