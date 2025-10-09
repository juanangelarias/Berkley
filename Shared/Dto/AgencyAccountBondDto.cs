namespace James.Shared.Dto;

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