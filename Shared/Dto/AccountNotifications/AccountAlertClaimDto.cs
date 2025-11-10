namespace James.Shared.Dto;

/// <summary>
/// Represents a Data Transfer Object (DTO) for account alert claims.
/// </summary>
/// <remarks>
/// This class encapsulates the details of a single account alert claim, including its unique identifier,
/// associated bond number, and its class code.
/// </remarks>
///
/// Used in:
///          AccountAlerts.razor (JamesWebUI.Client);
///          AccountAlertPackageDto.cs (James.Shared);
///          AccountAlerts.cs (James.Data.Server)
/// 
public class AccountAlertClaimDto
{
    public string ClaimId { get; set; } = "";
    public string BondNumber { get; set; } = "";
    public string ClassCode { get; set; } = "";
}