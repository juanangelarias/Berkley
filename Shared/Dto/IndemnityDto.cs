namespace James.Shared.Dto;

public class IndemnityDto
{
    public string AccountNum { get; set; } = string.Empty;
    public DateOnly AgreementDate { get; set; }
    public string? AgreementType { get; set; } = string.Empty;
    public string? AgreementForm { get; set; }
    public bool DocuSign { get; set; }

    public List<IndemnityDetailDto> Details { get; set; } = [];
}

public class IndemnityDetailDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int? NetLiquidAssets { get; set; }
    public int? NetWorth { get; set; }
    public int? IndemnityAmount { get; set; }
    public bool SpouseIndemnitor { get; set; }
    public string? EncryptSpouseTaxId { get; set; }
    public string? Signatory { get; set; }
    public string? Title { get; set; }
    public DateOnly? ExecutionDate { get; set; }
}