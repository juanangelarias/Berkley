using ApplicationLog;
using Auth0.AspNetCore.Authentication;
using Blazored.LocalStorage;
using James.Data.Imaging;
using James.Data.Server;
using James.Data.Server.GraphQL;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.Model;
using James.Shared;
using James.Shared.Constants;
using James.Shared.Data;
using James.Shared.Server;
using James.Shared.Server.Kong0;
using JamesWebUI.Client.Components;
using JamesWebUI.Client.Security;
using JamesWebUI.Client.Services;
using JamesWebUI.Server.Helpers;
using JamesWebUI.Server.SharedServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Server;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Radzen;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using FileInfo = System.IO.FileInfo;
using Path = System.IO.Path;
using Query = James.Data.Server.GraphQL.Queries.Query;
using ThemeService = JamesWebUI.Client.Services.ThemeService;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

try
{
    // Add services to the container.
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCascadingAuthenticationState();

    ConfirmAppSettingsEntry("Auth0:Authority");
    ConfirmAppSettingsEntry("Auth0:ClientId");
    ConfirmAppSettingsEntry("Auth0:ClientSecret");
    var auth0Authority = config["Auth0:Authority"]!;


    var domain = auth0Authority[(auth0Authority.IndexOf("://", StringComparison.Ordinal) + 3)..];
    builder.Services
        .AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = domain;
            options.ClientId = builder.Configuration["Auth0:ClientId"]!;
            options.ClientSecret = builder.Configuration["Auth0:ClientSecret"]!;
        })
        .WithAccessToken(options =>
        {
            options.Audience = builder.Configuration["Auth0:Audience"];
        });

    var kong0TokenUrl = new Uri(config["Kong0:token_url"] ?? "https://dev-auth-login.berkley.com/oauth/token");
    var tokenClientBuilder = builder.Services.AddHttpClient("P8FileNetTokens").ConfigureHttpClient(
        client =>
        {
            client.BaseAddress = kong0TokenUrl;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { ClientCertificateOptions = ClientCertificateOption.Automatic });
    if (bool.TryParse(config["Kong0:http_client_trace_logging"], out var traceLogging) && traceLogging)
        tokenClientBuilder.AddTraceContentLogging();

    builder.Services
        .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
        {
            o.EnableDetailedErrors();
            o.UseSqlServer(config.GetConnectionString("James"));
            o.EnableDetailedErrors();
            o.EnableSensitiveDataLogging();//TODO: Disable in production environment
            //o.UseMemoryCache()
        });
    builder.Services
        .AddGraphQLServer()
        .AddAuthorization()
        .AddQueryType<Query>()
        .RegisterDbContextFactory<JamesDatabaseContext>()
        .AddSubscriptionType<Subscription>()
        .AddJamesGraphQlTypes()
        .AddMutationConventions()
        .AddInMemorySubscriptions();
    
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.ConfigureApplicationCookie(options =>
    {
        // Default Lockout settings.
        options.LoginPath = JamesConstants.LOG_IN_PATH;
        options.LogoutPath = JamesConstants.LOG_OUT_PATH;
        options.ReturnUrlParameter = "redirectUri";
    });
    builder.Services.AddRadzenComponents();
    builder.Services.AddBlazoredLocalStorage(localStorageConfig =>
    {
        localStorageConfig.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    builder.Services.AddSignalR(e =>
    {
        e.EnableDetailedErrors = true;
        e.MaximumReceiveMessageSize = 1024*1024*4;//4MB since some cached values are over 3MB
    });
    builder.Services.AddScoped<ThemeService>()
                    .AddScoped<IUserShared, UserShared>()
                    .AddScoped<ILoggingShared, LoggingShared>()
                    .AddScoped<ILoggingService, ServerLoggingService>()
                    .AddScoped<ImagingKong0Helper>()
                    .AddScoped<ServerImagingAccess>()
                    .AddScoped<IDataAccess, ServerDataAccess>()
                    .AddScoped<IBrowserStorageCache, BlazorLocalStorageCache>()
                    .AddScoped<IUserSettingService, UserSettingService>()
                    .AddScoped<IAuthorizationHandler, RoleRequirementHandler>()
                    .AddSingleton<IAuthorizationPolicyProvider, RoleMembershipPolicyProvider>()
                    .AddSingleton<IAppEnvironment, ServerAppEnvironment>();

    if (OperatingSystem.IsWindows())
    {
        //Imaging Kong0 setup
        ConfirmAppSettingsEntry("Kong0:token_url");
        ConfirmAppSettingsEntry("Kong0:audience");
        ConfirmAppSettingsEntry("Kong0:Imaging:client_id");
        ConfirmAppSettingsEntry("Kong0:Imaging:client_secret");
        ConfirmAppSettingsEntry("Kong0:Imaging:service_url");
        builder.Services.SetupImagingForKong(config);
    }
    builder.Services.AddScoped<Query>();
    builder.Services.AddScoped<AccountMutation>();
    builder.Services.AddScoped<AgencyMutation>();
    builder.Services.AddScoped<AgentMutation>();
    builder.Services.AddScoped<ObligeeMutation>();
    builder.Services.AddScoped<GeneralMutation>();
    builder.Services.AddRazorComponents()
         .AddInteractiveServerComponents()
         .AddInteractiveWebAssemblyComponents()
         .AddAuthenticationStateSerialization(
             options =>
             {
                 options.SerializeAllClaims = true;
             }
         );
    builder.Services.AddScoped<AddressPhoneFormatService>();
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });

    builder.Services.AddHttpContextAccessor();
    //builder.Services.AddScoped<TokenHandler>();
    
    var corsSettings = config.GetSection("Cors").Get<CorsSettings>();
    //var corsSettings = new CorsSettings { Origins = "*" };
    
    builder.Services.AddCors(options =>
    {
        //TODO:  Make settings appropriate for production
        options.AddDefaultPolicy(policy =>
        {
            policy
                .WithOrigins(corsSettings?.Origins ?? "*")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    //Set up logging
    ConfirmAppSettingsEntry("ApplicationId");
    var loggerBuilder = builder.Logging
    //builder.Logging
    .AddConsole()
#if DEBUG
    .AddDebug()
#endif
    .AddSerilog(new LoggerConfiguration()
        .Enrich.WithApplicationInfo(config["ApplicationId"]!, "James")
        .ReadFrom.Configuration(config)
        .CreateLogger());
    if (OperatingSystem.IsWindows())
#pragma warning disable CA1416 // Validate platform compatibility
        loggerBuilder.AddEventLog(elSettings => elSettings.SourceName = "James");
#pragma warning restore CA1416 // Validate platform compatibility
    builder.Host.UseWindowsService();

    var app = builder.Build();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    if (!string.IsNullOrEmpty(config["Kestrel:Endpoints:Https:Url"]))
        app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();
    app.UseAntiforgery();

    app.UseAuthentication();
    app.UseAuthorization(); // Authorization ALWAYS after Authentication, both after UseRouting(); 

    app.UseWebSockets();

    app.MapGet(JamesConstants.LOG_IN_PATH, async (HttpContext httpContext, string redirectUri = "/") =>
    {
        //Note: Our Auth0 uses the ReturnUrl query value, even though the standard
        //          (even in Auth0 docs) is to use redirectUri
        var returnUrl = ReturnUrl(redirectUri, httpContext);
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);

    });

    app.MapGet(JamesConstants.LOG_OUT_PATH, async (HttpContext httpContext, string redirectUri = "/") =>
    {
        var returnUrl = ReturnUrl(redirectUri, httpContext);
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    });

    ConfirmAppSettingsEntry("Cors:Origins");
    var corsOriginString = config["Cors:Origins"] ?? "localhost,usilg01-isd076,usig01-isd076.wrbts.ads.wrberkley.com";
    app.UseCors(cors => cors.WithOrigins(corsOriginString.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)));

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.MapRazorPages();
    app.MapControllers();
    // Ensure static assets are mapped before interactive WebAssembly render mode
    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode();

    app.MapGraphQL();

    app.Run();
}
catch (Exception ex)
{
    var logFile = config["Serilog:WriteTo:0:Args:path"];
    var msg = "Exception starting service:\r\n" + ex;
    Console.WriteLine(msg);
    Debug.WriteLine(msg);
    if (!string.IsNullOrWhiteSpace(logFile))
    {
        var logDir = new FileInfo(logFile).DirectoryName;
        if (Directory.Exists(logDir))
        {
            var startupFailureFile = Path.Combine(logDir, "StartUpFailureException.txt");
            await using (var fs = new FileStream(startupFailureFile, FileMode.Create, FileAccess.Write))
            await using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(msg);
            }
        }
    }
}

string ReturnUrl(string redirectUri, HttpContext httpContext1)
{
    //HACK: Our Auth0 uses the ReturnUrl query value, even though the standard
    //          (even in Auth0 docs) is to use redirectUri
    var returnUrl1 = redirectUri;
    if (httpContext1.Request.Query.ContainsKey("ReturnUrl"))
        returnUrl1 = httpContext1.Request.Query["ReturnUrl"]!;

    return returnUrl1;
}

void ConfirmAppSettingsEntry(string key, string? message = null)
{
    IConfiguration configNode = config;
    try
    {
        foreach (var level in key.Split(':'))
        {
            configNode = configNode.GetRequiredSection(level);
        }
    }
    catch (InvalidOperationException)
    {
        if (message == null)
            throw new Exception(
                $"Configuration key '{key}' is required by was not found in appsettings.json or the environment variables");
        throw new Exception(message);
    }
}
