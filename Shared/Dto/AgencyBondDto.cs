namespace James.Shared.Dto;

public class AgencyBondDto
{
    public string Status { get; set; } = string.Empty;
    public string BondNumber { get; set; } = string.Empty;
    public string AccountNum { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string BondType { get; set; } = string.Empty;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid UnderWriterId { get; set; }
    public string UnderWriterName { get; set; } = string.Empty;
    public string SicCode { get; set; } = string.Empty;
    public Guid? ObligeeId { get; set; }
    public string ObligeeName { get; set; } = string.Empty;
    public string BondClass { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public int Amount { get; set; }
}