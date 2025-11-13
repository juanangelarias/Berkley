namespace JamesWebUI.Client.Model;

public class AccountGeneralInfoModel
{
    public DateTime? GIAExecutionDate { get; set; }
    public string? FiscalYearEnd { get; set; } = "";
    public string? BusinessType { get; set; } = "";
    public string? AccountIndustry { get; set; } = "";
    public string? PriorSuretyCompany { get; set; } = "";
    public bool IsSharedSurety { get; set; }
    public string? PrivateEquityText { get; set; }
    public string? PrivateEquityDate { get; set; }
    //ToDo: leaving the next field as a reminder to get the snapshot data (Read Only)
    public string SnapshotHistoricData { get; set; } = "";
}