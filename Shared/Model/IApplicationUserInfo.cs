namespace James.Shared.Model;

public interface IApplicationUserInfo
{
    string FullName { get; set; }
    string Initials { get; set; }
    string Title { get; set; }
    string Username { get; set; }
    bool IsUnderwriter { get; set; }
    bool IsHomeOfficeApprover { get; set; }
}