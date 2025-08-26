namespace JamesWebUI.Client.Helpers;

public static class CacheKeys
{
    public static string AccountKey(string accountNumber) => $"Account-{accountNumber}"; 
    public static string LegalEntityAddresses(Guid id) => $"LegalEntityAddresses-{id}";
    public static string LegalEntityPhoneNumbers(Guid id) => $"LegalEntityPhoneNumbers-{id}";
    public static string LegalEntityEmails(Guid id) => $"LegalEntityPhoneEmails-{id}";
    
    public const string AddressTypes = "AddressTypes";
    public const string Countries = "Countries";
    public const string EmailTypes = "EmailTypes";
    public const string PhoneTypes = "PhoneTypes";
    public const string States = "States";
}