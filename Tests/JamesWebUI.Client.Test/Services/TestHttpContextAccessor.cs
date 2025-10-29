using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace JamesWebUI.Client.Test.Services;

public class TestHttpContextAccessor : IHttpContextAccessor
{
    private HttpContext? _httpContext;
    public HttpContext? HttpContext
    {
        get
        {
            if (null == _httpContext)
            {
                _httpContext = new DefaultHttpContext();
                var testUserIdentity = new GenericIdentity("TestUser1234");
                testUserIdentity.AddClaim(new Claim("nickname", "TestUser1234"));
                _httpContext.User = new ClaimsPrincipal(testUserIdentity);
            }
            return _httpContext;
        }
        set => _httpContext = value;
    }
}