using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
 
namespace JamesWebUI.Client.Test;
 
public class TestAuthStateProvider(ClaimsPrincipal user) : AuthenticationStateProvider
{
    private ClaimsPrincipal _user = user;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(new AuthenticationState(_user));
 
    public void SetUser(ClaimsPrincipal user)
    {
        _user = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}