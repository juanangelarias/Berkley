namespace James.Shared.Dto;

public class UserInfoDto
{
    public Guid EmployeeId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsUnderwriter { get; set; }
}