using James.Shared.Model;

namespace James.Shared.Dto;

public class AgencyLocationsDto
{
    public bool IsTopParent { get; set; }
    public Agency Agency { get; set; } = null!;
}