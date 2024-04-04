using JamesWebUI.Client;
using JamesWebUI.Client.Components;
using JamesWebUI.Client.GraphQL;
using JamesWebUI.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using StrawberryShake;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("JamesAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


builder.Services.AddJamesClient(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(client =>
        client.BaseAddress = new Uri("https://localhost:7017/graphql"))

    .ConfigureWebSocketClient(client => client.Uri = new Uri("wss://localhost:7017/graphql"));

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("JamesAPI"));


builder.Services.AddRadzenComponents();
builder.Services.AddScoped<ThemeService>();

//builder.Services.AddSingleton<LoggingService>();


builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});



await builder.Build().RunAsync();
