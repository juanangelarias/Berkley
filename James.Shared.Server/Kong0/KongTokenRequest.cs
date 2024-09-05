using System.Text.Json.Serialization;

namespace James.Shared.Server.Kong0;

public class KongTokenRequest
{
    private static readonly Dictionary<Type, KongTokenRequest> _requests = new();
    public static KongTokenRequest SetRequest(Type type, KongTokenRequest request)
    {
        _requests[type] = request;
        return request;
    }

    public static KongTokenRequest GetRequest(Type type) => _requests[type];
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public string GrantType { get; set; } = "client_credentials";
    public required string Audience { get; set; }
}