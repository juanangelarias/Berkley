using James.Shared.Model;

namespace James.Shared.Dto;

public class AgencyAccountDto
{
    public string AccountNum { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Branch { get; set; } = null!;
    public string BranchFullName { get; set; } = null!;
    public Address? MainAddress { get; set; } = null!;
    public List<AgencyAccountBondDto> Bonds { get; set; } = [];
    public string Status { get; set; } = null!;
}