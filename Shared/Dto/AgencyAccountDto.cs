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

public class AgencyAccountBondDto
{
    public string BondNumber { get; set; } = null!;
    public DateTime Effective { get; set; }
    public DateTime Expiration { get; set; }
    public decimal Amount { get; set; }
    public Guid UnderwriterId { get; set; }
    public string UnderwriterFullName { get; set; } = null!;
    public Guid? ObligeeId { get; set; }
    public string? ObligeeFullName { get; set; }
    public string? BondType { get; set; }
    public string? Siccode { get; set; }
    public string? Municipality { get; set; }
    public string BondClass { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime Appointment { get; set; }
    public DateTime Termination { get; set; }
}