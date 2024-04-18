using ApplicationLog;
using James.Data.Server.Model;
using JamesWebUI.Client.Components;
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
        .AddQueryType<JamesWebUI.Server.GraphQL.Queries.Query>()
        .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled)
        .AddJamesGraphQlTypes();

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
    builder.Logging
    .AddConsole()
#if DEBUG
    .AddDebug()
#endif
    .AddEventLog(elSettings => elSettings.SourceName = "James")
    .AddSerilog(new LoggerConfiguration()
        .Enrich.WithApplicationInfo(config["ApplicationId"]!, "James")
        .ReadFrom.Configuration(config)
        .CreateLogger());
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

    app.UseAuthentication();
    app.UseAuthorization(); // Authorization ALWAYS after Authentication, both after UseRouting(); 
    app.UseAntiforgery();

    app.UseCors();
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
