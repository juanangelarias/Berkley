using James.Shared.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using James.Shared;

namespace JamesWebUI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private  ILogger<UserController> _logger;
        private readonly IUserShared _userShared;

        public UserController(ILogger<UserController> logger, IUserShared userShared)
        {
            _logger = logger;
            _userShared = userShared;
        }

        [HttpGet("/GetCurrentUserInfo")]
        public JsonResult GetCurrentUserInfo()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
                var userInfo = _userShared.GetCurrentUser().Result;
                Debug.WriteLine($"User info returned: {userInfo}");
                return new JsonResult(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in GetCurrentUserInfo()");
                return new JsonResult(ex.ToString()) { StatusCode = 500 };
            }
        }
    }
}
