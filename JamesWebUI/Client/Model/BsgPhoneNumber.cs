using James.Shared.Model;

namespace JamesWebUI.Client.Model;

public class BsgPhoneNumber: PhoneNumber
{
    public string Type { get; set; } = string.Empty;
    public bool IsNew { get; set; }
    public string CountryLabel => $"({CountryCode}) {CountryName}";
    public string CountryName { get; set; } = string.Empty;

    public BsgPhoneNumber()
    {
    }

    public BsgPhoneNumber(string type, PhoneNumber phoneNumber, bool isNew = false)
    {
        Id = phoneNumber.Id;
        Type = type;
        IsNew = isNew;
        CountryCode = phoneNumber.CountryCode;
        MainNumber = phoneNumber.MainNumber;
        Extension = phoneNumber.Extension;
    }
}