namespace JamesWebUI.Client.Model;

public class AccountGeneralInfoModel
{
    public DateTime? GIAExecutionDate { get; set; }
    public string? FiscalYearEnd { get; set; } = "";
    public string? BusinessType { get; set; } = "";
    public string? SicCode { get; set; } = "";
    public string? PriorSuretyCompany { get; set; } = "";
    public bool? IsSharedSurety { get; set; }
    public string PrivateEquity { get; set; } = "";
    public string SnapshotHistoricData { get; set; } = "";
}