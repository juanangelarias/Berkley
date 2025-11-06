namespace James.Shared.Dto;

/// <summary>
/// Represents an alert message associated with an account.
/// </summary>
/// <remarks>
/// This class typically contains details about the type of alert, a textual description,
/// the date of the alert, and an optional associated link.
/// </remarks>
///
/// Used in:
///         AccountAlerts.cs (James.Data.Server)
///         AccountAlertDialog.razor (JamesWebUI.Client)
///         AccountAlerts.razor (JamesWebUI.Client)
///         AccountAlertPackageDto.cs (James.Shared)
public class AccountAlertDto
{
    public string Type { get; set; } = "";
    public string Alert { get; set; } = "";
    public DateTime? Date { get; set; }
    public string Link { get; set; } = "";
    public string DateText => Date?.ToString("MM/dd/yyyy") ?? "";
}