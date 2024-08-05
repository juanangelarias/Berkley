using ApplicationLog;
using Auth0.AspNetCore.Authentication;
using James.Data.Server;
using James.Data.Server.GraphQL;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.Model;
using James.Shared;
using James.Shared.Data;
using James.Shared.Server;
using JamesWebUI.Client.Components;
using JamesWebUI.Client.Services;
using JamesWebUI.Server;
using JamesWebUI.Server.AuthenticationStateSyncer;
using JamesWebUI.Server.SharedServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using System.Diagnostics;
using FileInfo = System.IO.FileInfo;
using Path = System.IO.Path;
using Query = James.Data.Server.GraphQL.Queries.Query;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

try
{
    // Add services to the container.
    var builder = WebApplication.CreateBuilder(args);
    //Needed fpr Auth0 to work behind a reverse proxy
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

    var auth0Authority = config["Auth0:Authority"] ?? "https://dev-auth.wrberkley.auth0.com";
    builder.Services.AddHttpClient("Auth0UserInfo",
    client => client.BaseAddress = new Uri(auth0Authority))
        .AddHttpMessageHandler<TokenHandler>();
    builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("Auth0UserInfo"));

    var domain = auth0Authority[(auth0Authority.IndexOf("://", StringComparison.Ordinal) + 3)..];
    builder.Services
        .AddAuth0WebAppAuthentication(options =>
        {
            options.Domain = domain;
            options.ClientId = builder.Configuration["Auth0:ClientId"]!;
            options.ClientSecret = builder.Configuration["Auth0:ClientSecret"]!;
            //var reverseProxyUrl = config["ExternalReverseProxyUrl"];
            //if (!string.IsNullOrWhiteSpace(reverseProxyUrl))
            //{
            //    options.CallbackPath = reverseProxyUrl;
            //}
        })
        .WithAccessToken(options =>
        {
            options.Audience = builder.Configuration["Auth0:Audience"];
        });

    builder.Services
        .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
        {
            o.EnableDetailedErrors();
            o.UseSqlServer(config.GetConnectionString("James"));
            o.EnableDetailedErrors();
            o.EnableSensitiveDataLogging();//TODO: Disable in production environment
            //o.UseMemoryCache()
        });
    builder.Services.AddAuthorization();
    builder.Services
        .AddGraphQLServer()
        .AddAuthorization()
        .AddQueryType<Query>()
        .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled)
        .AddSubscriptionType<Subscription>()
        .AddJamesGraphQlTypes()
        .AddMutationConventions()
        .AddInMemorySubscriptions()
        ;
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
    builder.Services.AddScoped<ThemeService>();
    builder.Services.AddScoped<IUserShared, UserShared>();
    builder.Services.AddScoped<ILoggingShared, LoggingShared>();
    builder.Services.AddScoped<ILoggingService, ServerLoggingService>();
    builder.Services.AddScoped<IDataAccess, ServerDataAccess>();
    builder.Services.AddScoped<Query>();
    builder.Services.AddScoped<AgencyMutation>();
    builder.Services.AddScoped<ObligeeMutation>();
    builder.Services.AddRazorComponents()
         .AddInteractiveServerComponents()
         .AddInteractiveWebAssemblyComponents();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<TokenHandler>();
    builder.Services.AddCors(options =>
    {
        //TODO:  Make settings appropriate for production
        options.AddDefaultPolicy(policy =>
        {
            //HACK:  Not appropriate for production.
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });

    //Set up logging
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
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        app.UseDeveloperExceptionPage();
        app.UseForwardedHeaders();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseForwardedHeaders();
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

    //var serverSideLogger = (ILoggingService)app.Services.GetService(typeof(ILoggingService))!;
    app.MapGet(JamesConstants.LOG_IN_PATH, async (HttpContext httpContext, string redirectUri = "/") =>
    {
        //Note: Our Auth0 uses the ReturnUrl query value, even though the standard
        //          (even in Auth0 docs) is to use redirectUri
        var returnUrl = ReturnUrl(redirectUri, httpContext, config, app);
        var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);

    });

    app.MapGet(JamesConstants.LOG_OUT_PATH, async (HttpContext httpContext, string redirectUri = "/") =>
    {
        var returnUrl = ReturnUrl(redirectUri, httpContext, config, app);
        var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
            .WithRedirectUri(returnUrl)
            .Build();

        await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    });

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
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode();
    //.AddAdditionalAssemblies(typeof(App).Assembly)

    app.MapGraphQL("/graphql");

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

string ReturnUrl(string redirectUri, HttpContext httpContext1, IConfigurationRoot configurationRoot,
    WebApplication webApplication)
{
    //HACK: Our Auth0 uses the ReturnUrl query value, even though the standard
    //          (even in Auth0 docs) is to use redirectUri
    var returnUrl1 = redirectUri;
    if (httpContext1.Request.Query.ContainsKey("ReturnUrl"))
        returnUrl1 = httpContext1.Request.Query["ReturnUrl"]!;
    //var reverseProxyUrl = configurationRoot["ExternalReverseProxyUrl"];
    //if (!string.IsNullOrWhiteSpace(reverseProxyUrl))
    //{
    //    returnUrl1 = (reverseProxyUrl + returnUrl1).Replace("//", "/");
    //    //TODO:Remove after debugging Auth0 issue
    //    webApplication.Logger.LogInformation("Using ExternalReverseProxyUrl from appsettings.json for RedirectUri return Url = " + returnUrl1);
    //}

    return returnUrl1;
}
