namespace JamesWebUI.Client.Model;

public class AccountBasicInfoModel
{
    public string AccountNum { get; set; } = "";
    public string FullName { get; set; } = "";
    public Guid? UnderwriterId { get; set; }
    public string Branch { get; set; } = "";
    public string HoLead { get; set; } = "";
    public string Division { get; set; } = "";
    public string SicCode { get; set; } = "";
}