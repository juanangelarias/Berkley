using James.Shared;

namespace JamesWebUI.Server.SharedServices;

public class ServerAppEnvironment(IWebHostEnvironment env)
    : IAppEnvironment
{
    public bool IsDevelopment() => env.IsDevelopment();
}