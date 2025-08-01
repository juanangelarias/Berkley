namespace James.Shared.Model;
/// <summary>
/// Represents a member of a security role, which can be either a user or another role.
/// </summary>
public class SecurityRoleMember
{
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the user or role.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Type of the member, indicating whether it is a user or a role.
    /// </summary>
    public bool IsRole { get; set; }
}    