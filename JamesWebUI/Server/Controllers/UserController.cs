using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using James.Shared.Model;
using JamesWebUI.Server.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace JamesWebUI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private IHttpClientFactory _httpClientFactory;
        private HttpClient? _httpClient;

        private HttpClient UserInfoClient => _httpClient ??= _httpClientFactory.CreateClient("Auth0UserInfo");

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient("Auth0UserInfo");
        }

        private UserInformationCache<IAuth0UserInfo> CachedAuth0 = new UserInformationCache<IAuth0UserInfo>();

        [HttpGet("/GetCurrentUserInfo")]
        public JsonResult GetCurrentUserInfo()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token);
                Debug.WriteLine($"JWT encoded: {token}");
                Debug.WriteLine($"JWT decoded: {jwtSecurityToken}");
                if (UserInfoClient.DefaultRequestHeaders.Authorization != null)
                    UserInfoClient.DefaultRequestHeaders.Remove("Authorization");
                UserInfoClient.DefaultRequestHeaders.Add("Authorization",
                    Request.Headers[HeaderNames.Authorization].ToString());
                var userInfo = UserInfoClient.GetStringAsync("https://apps-sbox.wrberkley.auth0.com/userinfo").Result;
                Debug.WriteLine($"User infor returned: {userInfo}");
                return new JsonResult(userInfo);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new JsonResult(e.ToString());
            }
        }

        public async Task<SiteUserInfo> GetUserInfoAsync(string JWT)
        {
            //Get Email from JWT
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(JWT);
            var userInfoEndpoint = jwtSecurityToken.Audiences.First(a => a.StartsWith("http"));

            var siteUserInfo = new SiteUserInfo
            { JWT = JWT, Email = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.wrberkley.com/identity/application/claims/email")?.Value ?? "" };
            //Call Auth0's 
            if (UserInfoClient.DefaultRequestHeaders.Authorization != null)
                UserInfoClient.DefaultRequestHeaders.Remove("Authorization");
            UserInfoClient.DefaultRequestHeaders.Add("Authorization", "bearer " + JWT);
            var auth0Info = await UserInfoClient.GetFromJsonAsync<Auth0UserInfo>(userInfoEndpoint);
            //TODO: Do error handling
            siteUserInfo.FirstName = auth0Info.FirstName;
            siteUserInfo.LastName = auth0Info.LastName;
            siteUserInfo.FullName = auth0Info.Name;
            siteUserInfo.Username = auth0Info.Username;

            //var lookupTasks

            return siteUserInfo;
        }

        public async Task<IActiveDirectoryUserInfo> GetActiveDirectoryUserInfoAsync(string username)
        {
            var result = new List<GroupPrincipal>();

            await Task.Run(() =>
            {
                // establish domain context
                var yourDomain = new PrincipalContext(ContextType.Domain);

                // find your user
                var user = UserPrincipal.FindByIdentity(yourDomain, username);

                // if found - grab its groups
                if (user != null)
                {
                    var groups = user.GetAuthorizationGroups();

                    // iterate over all groups
                    foreach (var p in groups)
                    {
                        // make sure to add only group principals
                        if (p is GroupPrincipal)
                        {
                            result.Add((GroupPrincipal)p);
                        }
                    }
                }
            });
            return new ActiveDirectoryUserInformation
            { Username = username, ActiveDirectoryGroups = result.Select(gp => gp.Name).ToArray() };
        }

        public async Task<IApplicationUserInfo> GetApplicationUserInfoAsync(string username)
        {
            //TODO:This should be in the business logic layer.
            //UNDONE: Query SQL for the info
            throw new NotImplementedException();
        }
    }
}
