namespace James.Shared.Dto;

public class AccountProgramDto
{
    public Guid Id { get; set; }
    public string AccountNum { get; set; } = string.Empty;
    public DateTime Effective { get; set; }
    public DateTime Expiration { get; set; }
    public int Single { get; set; }
    public int Aggregate { get; set; }
    public Guid StatusId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; } = string.Empty;
    public DateTime? ApprovedDate { get; set; }
    public List<AccountProgramStatusLogDto> Logs { get; set; } = [];
}

public class AccountProgramStatusLogDto
{
    public Guid Id { get; set; }
    public Guid NewStatusId { get; set; }
    public string NewStatus { get; set; } = string.Empty;
    public Guid? OldStatusId { get; set; }
    public string? OldStatus { get; set; } = string.Empty;
    public int? OldSingle { get; set; }
    public int NewSingle { get; set; }
    public int? OldAggregate { get; set; }
    public int NewAggregate { get; set; }
    public DateTime StatusDate { get; set; }
    public Guid StatusChangeBy { get; set; }
    public string? StatusChangeByFullName { get; set; } = string.Empty;
}