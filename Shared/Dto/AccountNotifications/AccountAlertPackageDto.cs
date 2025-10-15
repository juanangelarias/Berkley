namespace James.Shared.Dto;

public class AccountAlertPackageDto
{
    public List<AccountAlertDto> Alerts { get; set; } = [];
    public List<AccountAlertClaimDto> Claims { get; set; } = [];
}