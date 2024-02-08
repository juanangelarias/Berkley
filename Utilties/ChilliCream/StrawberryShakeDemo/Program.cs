using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using StrawberryShakeDemo;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddGraphQlClient()
builder.Services.AddAccountClient()
    .ConfigureHttpClient(client => 
        client.BaseAddress = new Uri("https://localhost:7039/graphql"));
builder.Services.AddRadzenComponents();

await builder.Build().RunAsync();
