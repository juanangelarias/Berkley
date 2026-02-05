namespace James.Shared.Dto;

public class AccountLOADetailDto
{
    public string? BondType { get; set; }
    public DateTime Effective { get; set; }
    public DateTime Expiration { get; set; }
    public int Single { get; set; }
    public int Aggregate { get; set; }
    public string? Status { get; set; }
}

