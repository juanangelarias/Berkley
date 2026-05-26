namespace James.Shared.Dto;

public class AccountLOADto
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedByName { get; set; }
    public string? ApprovedByName { get; set; }
    public string AccountNum { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
    public DateTime Effective { get; set; }
    public DateTime Expiration { get; set; }
    public int LoaSingle { get; set; }
    public int LoaAggregate { get; set; }
    public string? Division { get; set; } = string.Empty;
    public string? BondType { get; set; } = string.Empty;
    public bool HomeOfficeApproved { get; set; }
    public string? Comments { get; set; } = string.Empty;
    public string? Conditions { get; set; } = string.Empty;
    public string? Status { get; set; } = string.Empty;
    public Guid? ReasonId { get; set; }
}