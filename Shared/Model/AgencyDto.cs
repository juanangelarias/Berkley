namespace James.Shared.Model;

public class AgencyDto
{
    public Guid Id { get; set; }
    public string AgencyNumber { get; set; } = "";
    public string FullName { get; set; } = "";
    public string FullNameDisplay { get; set; } = "";
    public string Status { get; set; } = "";
}