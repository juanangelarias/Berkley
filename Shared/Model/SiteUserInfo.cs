using System.Text.Json.Serialization;

namespace James.Shared.Model
{
    public partial class SiteUserInfo : IActiveDirectoryUserInfo, IJwtUserInfo, IAuth0UserInfo, IApplicationUserInfo
    {
        public string EntraId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string PictureUrl { get; set; } = string.Empty;
        public bool IsUnderwriter { get; set; } = false;
        public bool IsHomeOfficeApprover { get; set; } = false;
        public string JWT { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string[] ActiveDirectoryGroups { get; set; } = [];
    }

    public class Auth0UserInfo : IAuth0UserInfo
    {
        //public string JWT { get; set; }
        [JsonPropertyName("EntraId")]
        public string EntraId { get; set; } = string.Empty;

        [JsonPropertyName("given_name")]
        public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("family_name")]
        public string LastName { get; set; } = string.Empty;
        [JsonPropertyName("nickname")]
        public string Username { get; set; } = string.Empty;
        [JsonPropertyName("name")]
        public string FullName { get; set; } = string.Empty;
        [JsonPropertyName("sub")]
        public string Subject { get; set; } = string.Empty;
        [JsonPropertyName("updated_at")]
        public DateTime Updated { get; set; }
    }

    public class ActiveDirectoryUserInformation:IActiveDirectoryUserInfo
    {
        public string Username { get; set; } = string.Empty;
        public string[] ActiveDirectoryGroups { get; set; } = [];
    }

    public class ApplicationUserInformation : IApplicationUserInfo
    {
        public string FullName { get; set; } = string.Empty;
        public string Initials { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsUnderwriter { get; set; }
        public bool IsHomeOfficeApprover { get; set; }
    }

    public class ActiveDirectoryGroupMembership : IActiveDirectoryGroupMembership
    {
        public string ActiveDirectoryGroup { get; set; } = string.Empty;
        public string[] Members { get; set; } = [];
    }

}
