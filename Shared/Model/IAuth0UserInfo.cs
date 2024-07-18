using System.Text.Json.Serialization;

namespace James.Shared.Model;

public interface IAuth0UserInfo
{
    string EntraId { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Username { get; set; }
    string FullName { get; set; }
}