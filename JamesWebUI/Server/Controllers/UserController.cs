using System.Collections;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Versioning;
using James.Shared.Model;
using JamesWebUI.Server.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
            CachedAuth0 ??= new UserInformationCache<IAuth0UserInfo> { LookupTask = GetAuth0UserInfo };
            CachedActiveDirectory ??= new UserInformationCache<IActiveDirectoryUserInfo>
            {
                LookupTask = GetActiveDirectoryUserInfoAsync,
                CacheDuration = TimeSpan.FromHours(9)
            };
        }

        private static UserInformationCache<IAuth0UserInfo>? CachedAuth0;
        private static UserInformationCache<IActiveDirectoryUserInfo>? CachedActiveDirectory;
        //private static UserInformationCache<IApplicationUserInfo>? CachedApplication;
        private static UserInformationCache<IActiveDirectoryGroupMembership>? CachedActiveDirectoryGroupMembership;
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

        [HttpGet("/GetActiveDirectoryGroupMemebership/{groupName}")]
        public JsonResult GetActiveDirectoryGroupMemebership(string groupName)
        {
            try
            {
                //var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
                //var userInfo = GetUserInfoAsync(token).Result;
                var groupMembership = CachedActiveDirectoryGroupMembership.GetAsync(groupName).Result;
                Debug.WriteLine(
                    $"AD Group Membership returned for {groupMembership.ActiveDirectoryGroup}:\r\n{string.Join("\r\n", groupMembership.Members)}");
                return new JsonResult(groupMembership);
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
                return userInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(13321, "GetUserInfoError"), ex, "Exception thrown while getting Auth0 user information.");
                throw;
            }
        }

        public async Task<SiteUserInfo> GetUserInfoAsync(string JWT)
        {
            _logger.LogDebug("UserInfo request received.");
            
            //Get Email from JWT
            JwtSecurityToken jwtSecurityToken;
            SiteUserInfo siteUserInfo;
            try
            {
                jwtSecurityToken = _handler.ReadJwtToken(JWT);
                //var userInfoEndpoint = jwtSecurityToken.Audiences.First(a => a.StartsWith("http"));

                siteUserInfo = new SiteUserInfo
                {
                    JWT = JWT,
                    Email = jwtSecurityToken.Claims.FirstOrDefault(c =>
                        c.Type == "http://schemas.wrberkley.com/identity/application/claims/email")?.Value ?? ""
                };
                //Call Auth0's 
                if (UserInfoClient.DefaultRequestHeaders.Authorization != null)
                    UserInfoClient.DefaultRequestHeaders.Remove("Authorization");
                UserInfoClient.DefaultRequestHeaders.Add("Authorization", "bearer " + JWT);
                Debug.Assert(CachedAuth0 != null, nameof(CachedAuth0) + " != null");
                var auth0Info = await CachedAuth0.GetAsync(JWT);

                //TODO: Do error handling
                siteUserInfo.FirstName = auth0Info.FirstName;
                siteUserInfo.LastName = auth0Info.LastName;
                siteUserInfo.FullName = auth0Info.FullName;
                siteUserInfo.Username = auth0Info.Username;

                //Query Active Directory and application database (if needed)
                var adLookup = CachedActiveDirectory.GetAsync(siteUserInfo.Username);
                //var appLookup = GetApplicationUserInfoAsync(siteUserInfo.Username);
                var lookupTasks = new Task[]
                {
                    adLookup
                };
                Task.WaitAll(lookupTasks);
                var activeDirectoryUserInformation = adLookup.Result;
                siteUserInfo.ActiveDirectoryGroups = activeDirectoryUserInformation.ActiveDirectoryGroups;
                //var applicationUserInformation = appLookup.Result;
                //siteUserInfo.Initials = applicationUserInformation.Initials;
                //siteUserInfo.IsHomeOfficeApprover = applicationUserInformation.IsHomeOfficeApprover;
                //siteUserInfo.IsUnderwriter = applicationUserInformation.IsUnderwriter;
                //siteUserInfo.Title = applicationUserInformation.Title;
                //TODO: If your application database keeps a different fullname than what is in Auth0, below is where you would update it
                //siteUserInfo.FullName = applicationUserInformation.FullName?? siteUserInfo.FullName;
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(13321, "GetUserInfoError"), e,
                    "Exception thrown while getting user information.");
                throw;
            }

            return siteUserInfo;
        }

        /// <summary>
        /// Retrieves group information from Active Directory
        /// </summary>
        /// <param name="username">User to get group information for</param>
        /// <returns>A list of all active directory groups the user is a member of</returns>
        /// <remarks>Getting all groups is hideously slow.  It would be much better to either check membership for a specific set of Groups,
        ///             or to get and cache a list of all members of the relevant groups.</remarks>

        [SupportedOSPlatform("windows")]
        public static async Task<IActiveDirectoryUserInfo> GetActiveDirectoryUserInfoAsync(string username)
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
            Debug.WriteLine("Returning AD info");
            return new ActiveDirectoryUserInformation
            { Username = username, ActiveDirectoryGroups = result.Select(gp => gp.Name).ToArray() };
        }

        //public static async Task<IApplicationUserInfo> GetApplicationUserInfoAsync(string username)
        //{
        //    //TODO:This should come from the application database.  Simplified mockup for the demo
        //    await Task.Delay(1);
        //    return new ApplicationUserInformation
        //    {
        //        FullName = "Public, John Q.",
        //        Initials = "JQP",
        //        IsHomeOfficeApprover = false,
        //        IsUnderwriter = true,
        //        Title = "Example Underwriter",
        //        Username = username
        //    };
        //}


        //[SupportedOSPlatform("windows")]
        //public async Task<IActiveDirectoryGroupMembership> GetActiveDirectoryGroupMembers(string groupName)
        //{
        //    var yourDomain = new PrincipalContext(ContextType.Domain);
        //    GroupPrincipal group;
        //    var memberList = new List<string>();
        //    await Task.Run(() =>
        //    {
        //        group = GroupPrincipal.FindByIdentity(yourDomain, groupName);
        //        if (group != null)
        //        {
        //            var groupQueue = new Queue<GroupPrincipal>();
        //            groupQueue.Enqueue(group);
        //            while (groupQueue.Count > 0)
        //            {
        //                var grp = groupQueue.Dequeue();
        //                foreach (var member in grp.GetMembers())
        //                {
        //                    if (member is GroupPrincipal gp)
        //                        groupQueue.Enqueue(gp);
        //                    else if (member is UserPrincipal user)
        //                        memberList.Add(user.UserPrincipalName.Split("@").First());
        //                    else
        //                        Debug.WriteLine("Was not expecting type " + member.GetType());
        //                }
        //            }
        //        }
        //    });
        //    return new ActiveDirectoryGroupMembership
        //    { ActiveDirectoryGroup = groupName, Members = memberList.ToArray() };
        //}
    }
}
