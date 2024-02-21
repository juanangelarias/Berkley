using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using StrawberryShake;
using StrawberryShakeWASM.Client;


//Debug.WriteLine("Beginning client program.cs");
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//Radzen Blazor setup
builder.Services.AddRadzenComponents();

//StrawberryShake
builder.Services.AddAccountClient(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(client => 
        client.BaseAddress = new Uri("https://localhost:7039/graphql"))
    .ConfigureWebSocketClient(client => client.Uri = new Uri("wss://localhost:7039/graphql"));

//Automapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

await builder.Build().RunAsync();
