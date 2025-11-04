namespace James.Shared.Model;

public class AgencyDto
{
    public Guid Id { get; set; }
    public string AgencyNumber { get; set; } = "";
    public string FullName { get; set; } = "";
    public string FullNameDisplay => $"({AgencyNumber}) {FullName}";
    public string Status { get; set; } = "";
}