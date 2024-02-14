using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using StrawberryShake;
using StrawberryShakeWASM.Client;


//Debug.WriteLine("Beginning client program.cs");
var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//StrawberyShake
builder.Services.AddAccountClient(ExecutionStrategy.CacheAndNetwork)
    .ConfigureHttpClient(client => 
        client.BaseAddress = new Uri("https://localhost:7039/graphql"))

    .ConfigureWebSocketClient(client => client.Uri = new Uri("ws://localhost:7039/graphql"));

//Radzen Blazor setup
builder.Services.AddRadzenComponents();
//builder.Services.AddScoped<DialogService>();
//builder.Services.AddScoped<TooltipService>();
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<ContextMenuService>();

//Debug.WriteLine("Beginning client automapper config");
//Automapper
//Mapper.Initialize(cfg=>)
builder.Services.AddAutoMapper(typeof(MapperProfile));
//var mapperConfig = new MapperConfiguration(mc =>
//{
//    mc.AddProfile(typeof(MapperProfile));
//});
//IMapper mapper = mapperConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);

await builder.Build().RunAsync();
