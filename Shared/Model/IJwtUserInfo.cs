namespace James.Shared.Model;

public interface IJwtUserInfo
{
    string JWT { get; set; }
    string Email { get; set; }
}