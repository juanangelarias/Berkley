namespace James.Shared.Dto;

public class AccountAgencyLOADto()
{
    public Guid Id { get; set; }
    public DateTime Effective { get; set; }
    public DateTime Expiration { get; set; }
    public int LoaSingle { get; set; }
    public int LoaAggregate { get; set; }
    public string? Division { get; set; } = string.Empty;
    public string? BondType { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public int SequenceNumber { get; set; }
    public Guid? CreatedById { get; set; }
    public string? CreatedByName { get; set; } = string.Empty;
}