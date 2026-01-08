namespace James.Shared.Dto;

public class AccountOutstandingLiabilityByTypeAndClassDto
{
    public string BondType { get; set; } = "";
    public string BondClass { get; set; } = "";
    public int Amount { get; set; }
}