using James.Shared.Model;

namespace James.Shared.Dto;

public class AccountAbstractDto
{
    public string AccountNum { get; set; } = string.Empty;
    public string? PrivateOrPublic { get; set; } = string.Empty;
    public int CurrentOutstandingLiability { get; set; } = 0;
    public string SoleShareCoSurety { get; set; } = string.Empty;
    public string CoSureties { get; set; } = string.Empty;
    public int Collateral { get; set; } = 0;
    public int PotentialPremium { get; set; } = 0;
    public string Rate { get; set; } = string.Empty;
    public string Commission { get; set; } = string.Empty;
    public string Underwriter { get; set; } = string.Empty;
    public string AgencyName { get; set; } = string.Empty;
    public string AgentName { get; set; } = string.Empty;
    public CreditReportHistory? LastCreditReport { get; set; }
}