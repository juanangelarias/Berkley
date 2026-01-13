using James.Shared.Model;

namespace James.Shared.Dto;

public class AccountRateAndCommissionDto
{
    public List<string> RateGroups { get; set; } = [];      // RateGroupDM
    
    // Contract
    public List<string> RateClasses { get; set; } = [];
    public List<string> RateTypes { get; set; } = [];
    public List<ContractRate> ContractRates { get; set; } = [];
    
    // Commercial
    public List<string> CommercialBondTypes { get; set; } = [];       // CommercialBondTypeDM
    public List<string> RiskTypes { get; set; } = [];       // RiskTypeDM
    public List<CommercialRate> CommercialRates { get; set; } = [];
}