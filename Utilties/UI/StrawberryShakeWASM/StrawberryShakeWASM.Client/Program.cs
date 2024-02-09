using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddAccountClient()
    .ConfigureHttpClient(client => 
        client.BaseAddress = new Uri("https://localhost:7039/graphql"))

    .ConfigureWebSocketClient(client => client.Uri = new Uri("ws://localhost:7039/graphql"));
builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
