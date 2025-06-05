using James.Shared.Server;
using James.Shared;
using JamesAdminUI.Components;
using JamesWebUI.Shared.Services;
using System.Diagnostics;
using ApplicationLog;
using Serilog;
using Radzen;
using FileInfo = System.IO.FileInfo;
using ThemeService = JamesWebUI.Shared.Services.ThemeService;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
try
{
    // Add services to the container.
var builder = WebApplication.CreateBuilder(args);
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
    builder.Services.AddAuthorization();

    builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    builder.Services.AddRadzenComponents();
    builder.Services.AddScoped<ThemeService>();
    builder.Services.AddScoped<IUserShared, UserShared>();
    builder.Services.AddScoped<ILoggingShared, LoggingShared>();
    builder.Services.AddScoped<ILoggingService, ServerLoggingService>();
    builder.Host.UseWindowsService();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();


    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

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
