using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using JamesWebUI.Server.Services;

namespace JamesWebUI.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private  ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("/GetCurrentUserInfo")]
        public JsonResult GetCurrentUserInfo()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
                var userInfo = _userService.GetUserInfoAsync(token).Result;
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
