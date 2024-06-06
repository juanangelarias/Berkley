using James.Data.Client;
using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Data;
using JamesWebUI.Client;
using JamesWebUI.Client.Services;
using JamesWebUI.Client.AuthenticationStateSyncer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using StrawberryShake;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddHttpClient("JamesAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
    ;
builder.Services.AddHttpClient(JamesClient.ClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>()
    ;

var graphqlHttpUrl = builder.HostEnvironment.BaseAddress + "graphql";
var graphqlWebSocketUrl = graphqlHttpUrl.Replace("http", "ws", StringComparison.InvariantCultureIgnoreCase);
builder.Services.AddJamesClient(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(graphqlHttpUrl))
    .ConfigureWebSocketClient(client => client.Uri = new Uri(graphqlWebSocketUrl));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("JamesAPI"));


builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddSingleton<ILoggingService, LoggingService>();
builder.Services.AddScoped<IDataAccess,ClientDataAccess>();


builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]!);
});

await builder.Build().RunAsync();
