using Microsoft.AspNetCore.Authorization;

namespace JamesWebUI.Client.Security
{
    public class RoleRequirement(string role) : IAuthorizationRequirement
    {
        public string Role { get; } = role;
    }
}
