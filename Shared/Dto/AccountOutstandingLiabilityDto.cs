namespace James.Shared.Dto;

public class AccountOutstandingLiabilityDto
{
    public int OutstandingCommercialLiability { get; set; }
    public int OutstandingContractLiability { get; set; }
    public int LargestBondEver { get; set; }
    public List<AccountOutstandingLiabilityByTypeAndClassDto> LargestOutstandingBonds { get; set; } = [];
}