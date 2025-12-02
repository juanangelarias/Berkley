using James.Shared.Model;

namespace James.Shared.Dto;

public class AgencySearchDto
{
    public Guid Id { get; set; }
    public string AgencyNumber { get; set; } = string.Empty;
    public string AgencyName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ParentChild { get; set; } = string.Empty;
    public LegalEntity IdNavigation { get; set; }
}