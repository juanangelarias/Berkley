using James.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace JamesWebUI.Client.Classes;

public class ClientAppEnvironment(IWebAssemblyHostEnvironment env) 
    : IAppEnvironment
{
    public bool IsDevelopment() => env.IsDevelopment();
}