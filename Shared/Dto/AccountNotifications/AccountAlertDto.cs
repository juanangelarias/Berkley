namespace James.Shared.Dto;

public class AccountAlertDto
{
    public string Type { get; set; } = "";
    public string Alert { get; set; } = "";
    public DateTime? Date { get; set; }
    public string Link { get; set; } = "";
    public string DateText => Date?.ToString("MM/dd/yyyy") ?? "";
}