namespace James.Shared.Model;

public interface IActiveDirectoryUserInfo
{
    string Username { get; set; }
    string[] ActiveDirectoryGroups { get; set; }
}