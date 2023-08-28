using System.Text.Json.Serialization;

namespace James.Shared.Model;

public interface IAuth0UserInfo
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string Username { get; set; }
    string FullName { get; set; }
}