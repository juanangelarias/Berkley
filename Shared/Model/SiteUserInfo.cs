using System.Text.Json.Serialization;

namespace James.Shared.Model
{
    public partial class SiteUserInfo : IActiveDirectoryUserInfo, IJwtUserInfo, IAuth0UserInfo, IApplicationUserInfo
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Initials { get; set; }
        public string? Title { get; set; }
        public string? PictureUrl { get; set; }
        public bool IsUnderwriter { get; set; } = false;
        public bool IsHomeOfficeApprover { get; set; } = false;
        public string JWT { get; set; }
        public string Email { get; set; }
        public string[] ActiveDirectoryGroups { get; set; } = Array.Empty<string>();
    }

    public class Auth0UserInfo : IAuth0UserInfo
    {
        //public string JWT { get; set; }
        [JsonPropertyName("given_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("family_name")]
        public string LastName { get; set; }
        [JsonPropertyName("nickname")]
        public string Username { get; set; }
        [JsonPropertyName("name")]
        public string FullName { get; set; }
        [JsonPropertyName("sub")]
        public string Subject { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime Updated { get; set; }
    }

    public class ActiveDirectoryUserInformation:IActiveDirectoryUserInfo
    {
        public string Username { get; set; }
        public string[] ActiveDirectoryGroups { get; set; }
    }

    public class ApplicationUserInformation : IApplicationUserInfo
    {
        public string? FullName { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public bool IsUnderwriter { get; set; }
        public bool IsHomeOfficeApprover { get; set; }
    }

    public class ActiveDirectoryGroupMembership : IActiveDirectoryGroupMembership
    {
        public string ActiveDirectoryGroup { get; set; }
        public string[] Members { get; set; }
    }

}
