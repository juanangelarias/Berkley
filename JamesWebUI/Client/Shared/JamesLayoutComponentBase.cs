using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace JamesWebUI.Client.Shared
{
    public abstract class JamesLayoutComponentBase:LayoutComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState>? AuthenticationState { get; set; }

        private Task<AuthenticationState> _loadAuthenticationStateAsync { get; set; }
        //TODO: Discuss whether to allow only synchronous or asynchronous access to values.
        //Synchronous access properties
        protected AuthenticationState ? State { get; private set; }
        protected ClaimsPrincipal? UserPrincipal => State?.User;
        protected bool IsAuthenticationStateLoaded { get; private set; }
        protected bool IsAuthenticated=>IsAuthenticationStateLoaded && State!.User.Identity != null;

        //Asynchronous access to properties
        protected async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_loadAuthenticationStateAsync.IsCompleted)
                return _loadAuthenticationStateAsync.Result;
            return await _loadAuthenticationStateAsync;
        }

        protected async Task<ClaimsPrincipal> GetUserPrincipalAsync()
        {
            if (_loadAuthenticationStateAsync.IsCompleted)
                return _loadAuthenticationStateAsync.Result.User;
            return (await _loadAuthenticationStateAsync).User;
        }

        protected async Task<bool> GetIsAuthenticatedAsync()
        {
            if (_loadAuthenticationStateAsync.IsCompleted)
                return null != _loadAuthenticationStateAsync.Result.User.Identity;
            return null != (await _loadAuthenticationStateAsync).User.Identity;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (null != AuthenticationState)
                //NOTE:Running in background can cause issues if values are queried immediately upon return.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                {
                    _loadAuthenticationStateAsync = AuthenticationState;
                    State = await AuthenticationState;
                    IsAuthenticationStateLoaded = true;
                });
        }
    }

    public static class AuthUserExtensions
    {
        public static string? Username(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;
        }

        public static string? EmailAddress(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "email_address")?.Value;
        }
        public static string? FirstName(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        }
        public static string? LastName(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
        }
        public static string? PictureUrl(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
        }
        public static string? UpdatedAt(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "updated_at")?.Value;
        }
        public static string? NameIdentifier(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "email_address")?.Value;
        }
        public static string? SID(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "sid")?.Value;
        }
    }
}
