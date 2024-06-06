namespace JamesWebUI.Client.AuthenticationStateSyncer
{
    using James.Shared.Model;
    using JamesWebUI.Client.Pages;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using System.Security.Claims;

    public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> _unauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!persistentState.TryTakeFromJson<SiteUserInfo>(nameof(SiteUserInfo), out var userInfo) || userInfo is null)
            {
                return _unauthenticatedTask;
            }

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, userInfo.Username ?? string.Empty),
                new Claim(ClaimTypes.Name, userInfo.FullName ?? string.Empty),
                new Claim(ClaimTypes.Email, userInfo.Email)];

            return Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(PersistentAuthenticationStateProvider)))));
        }
    }
}
