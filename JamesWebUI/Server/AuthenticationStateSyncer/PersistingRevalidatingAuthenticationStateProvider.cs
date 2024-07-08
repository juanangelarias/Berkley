using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using James.Shared.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace JamesWebUI.Server.AuthenticationStateSyncer;

public class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PersistentComponentState _state;
    private readonly IdentityOptions _options;
    private readonly IHttpContextAccessor _contextAccessor;

    private readonly PersistingComponentStateSubscription _subscription;

    private Task<AuthenticationState>? _authenticationStateTask;

    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        PersistentComponentState state,
        IOptions<IdentityOptions> options,
        IHttpContextAccessor contextAccessor)
        : base(loggerFactory)
    {
        _scopeFactory = scopeFactory;
        _state = state;
        _options = options.Value;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        _contextAccessor = contextAccessor;
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the user manager from a new scope to ensure it fetches fresh data
        await using var scope = _scopeFactory.CreateAsyncScope();
        return ValidateSecurityStampAsync(authenticationState.User);
    }

    private bool ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated is false)
        {
            return false;
        }

        return true;
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
    {
        _authenticationStateTask = authenticationStateTask;
    }

    private async Task OnPersistingAsync()
    {
        if (_authenticationStateTask is null)
        {
            throw new UnreachableException($"Authentication state not set in {nameof(RevalidatingServerAuthenticationStateProvider)}.{nameof(OnPersistingAsync)}().");
        }

        var authenticationState = await _authenticationStateTask;
        var principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated == true)
        {
            //TODO: set up to use the UserShared code to hydrate the SiteUserInfo
            var userId = principal.FindFirst("nickname")?.Value;
            var name = principal.FindFirst("name")?.Value;
            var email = principal.FindFirst("email_address")?.Value;
            var jwt = _contextAccessor.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString().Split(" ").Last();
            if (string.IsNullOrEmpty(jwt))
            {
                Debug.Assert(_contextAccessor.HttpContext != null, "_contextAccessor.HttpContext != null");
                var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
                if (accessToken != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(accessToken);
                    jwt = token.ToString();
                }
            }
            if (userId != null && name != null)
            {
                _state.PersistAsJson(nameof(SiteUserInfo), new SiteUserInfo
                {
                    Username = userId,
                    FullName = name,
                    Email = email!,
                    PictureUrl = principal.FindFirstValue("picture"),
                    JWT = jwt ?? ""
                });
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        _subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
}