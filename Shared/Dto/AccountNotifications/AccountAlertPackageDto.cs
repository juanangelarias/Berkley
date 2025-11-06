namespace James.Shared.Dto;

/// <summary>
/// Represents a package containing account alert details including alerts and claims.
/// </summary>
/// <remarks>
/// This Data Transfer Object (DTO) is designed to encapsulate information related to account alerts
/// and their associated claims. It provides collections for managing critical account-related
/// notifications and claims data.
/// </remarks>
public class AccountAlertPackageDto
{
    public List<AccountAlertDto> Alerts { get; set; } = [];
    public List<AccountAlertClaimDto> Claims { get; set; } = [];
}