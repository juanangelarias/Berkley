namespace James.Shared.Dto;

public class AccountBondedPrincipleDto
{
    public RelatedAccountDto ParentAccount { get; set; } = new();
    public List<RelatedAccountDto> RelatedAccounts { get; set; } = [];
}