using ApplicationLog;
using James.Data.Server.Model;
using JamesWebUI.Client.Components;
using JamesWebUI.Server.Controllers;
using JamesWebUI.Server.GraphQL;
using JamesWebUI.Server.SharedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Serilog;
using FileInfo = System.IO.FileInfo;
using Path = System.IO.Path;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

try
{
    // Add services to the container.
    var builder = WebApplication.CreateBuilder(args);

    var auth0Authority = config["Auth0:Authority"] ?? "https://dev-auth.wrberkley.auth0.com";
    builder.Services.AddHttpClient("Auth0UserInfo",
        client => client.BaseAddress = new Uri("https://apps-sbox.wrberkley.auth0.com/"));
    builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
        .CreateClient("Auth0UserInfo"));


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:ApiIdentifier"];
    });

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();
    builder.Services.AddRadzenComponents();
    builder.Services.AddRazorComponents()
        .AddInteractiveWebAssemblyComponents();

    builder.Services
        .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
        {
            o.EnableDetailedErrors();
            o.UseSqlServer(config.GetConnectionString("James"));
            //o.UseMemoryCache()
        });
    builder.Services
        .AddGraphQLServer()
        .AddAuthorization()
        .AddQueryType<JamesWebUI.Server.GraphQL.Queries.Query>()
        .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled)
        .AddSubscriptionType<Subscription>()
        .AddJamesGraphQlTypes()
        .AddMutationConventions()
        .AddInMemorySubscriptions()
        ;
    builder.Services.AddScoped<IUserShared, UserShared>();
    builder.Services.AddHttpContextAccessor();
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

    //TODO: Move CORS config to either config file or environment variable
    app.UseCors(cors => cors.WithOrigins(new[]{"localhost", "usilg01-isd076", "usig01-isd076.wrbts.ads.wrberkley.com" +
                                                                               ""}));
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
        .AddInteractiveWebAssemblyRenderMode();

    app.MapGraphQL("/graphql");

    app.Run();
}
catch (Exception ex)
{
    var logFile = config["Serilog:WriteTo:0:Args:path"];
    var msg = "Exception starting service:\r\n" + ex;
    Console.WriteLine(msg);
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
