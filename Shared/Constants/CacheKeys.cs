namespace James.Shared.Constants;

public static class CacheKeys
{
    // Calculated
    // A
    public static string AccountKey(string accountNumber) => $"Account-{accountNumber}";
    public static string AgencyAgents(Guid id) => $"AgencyAgents-{id}";
    // L
    public static string LegalEntityAddresses(Guid id) => $"LegalEntityAddresses-{id}";
    public static string LegalEntityEmails(Guid id) => $"LegalEntityPhoneEmails-{id}";
    public static string LegalEntityPhoneNumbers(Guid id) => $"LegalEntityPhoneNumbers-{id}";
    
    // Fixed
    // A
    public const string AddressTypes = "AddressTypes";
    public const string AgenciesDto = "AgenciesDto";
    public const string AgencyStatuses = "AgencyStatuses";
    // B
    public const string Branches = "Branches";
    // C
    public const string Countries = "Countries";
    // D
    public const string Divisions = "Divisions";
    // E
    public const string EmailTypes = "EmailTypes";
    // N
    public const string Notifications = "Notifications";
    public const string NotificationPriorities = "NotificationPriorities";
    // P
    public const string PhoneTypes = "PhoneTypes";
    // S
    public const string States = "States";
    // U
    public const string Underwriters = "Underwriters";
    // W
    public const string WatchStatuses = "WatchStatuses";
}