using Blazored.LocalStorage;
using James.Data.Client;
using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Data;
using JamesWebUI.Client.Classes;
using JamesWebUI.Client.Security;
using JamesWebUI.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using StrawberryShake;
using System.Text.Json.Serialization;
using James.Shared.Model;
using Microsoft.Extensions.Options;
using ThemeService = JamesWebUI.Client.Services.ThemeService;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddHttpClient("JamesAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient(JamesClient.ClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

var graphqlHttpUrl = builder.HostEnvironment.BaseAddress + "graphql";
var graphqlWebSocketUrl = graphqlHttpUrl.Replace("http", "ws", StringComparison.InvariantCultureIgnoreCase);
builder.Services.AddJamesClient(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(graphqlHttpUrl))
    .ConfigureWebSocketClient(client => client.Uri = new Uri(graphqlWebSocketUrl));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("JamesAPI"));

builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorageAsSingleton(config =>
{
    config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services
    .AddScoped<AddressPhoneFormatService>()
    .AddScoped<ThemeService>()
    .AddSingleton<ILoggingService, LoggingService>()
    .AddSingleton<IDataAccess, ClientDataAccess>()
    .AddSingleton<IBrowserStorageCache, BlazorLocalStorageCache>()
    .AddScoped<IUserSettingService, UserSettingService>()
    .AddSingleton<IAuthorizationHandler, RoleRequirementHandler>()
    .AddSingleton<IAuthorizationPolicyProvider, RoleMembershipPolicyProvider>()
    .AddSingleton<IAppEnvironment, ClientAppEnvironment>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]!);
});

await builder.Build().RunAsync();