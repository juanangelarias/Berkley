namespace James.Shared.Constants;

/// <summary>
/// Provides a collection of keys used for caching operations across the application.
/// This class contains both constant and dynamic methods for generating cache keys.
/// </summary>
public static class CacheKeys
{
    // Calculated
    // A
    public static string Account(string accountNumber) => $"Account-{accountNumber}";
    public static string AccountAlerts(string alertPeriod, string accountNumber) => $"AccountAlerts-{alertPeriod}-{accountNumber}";
    public static string AccountAnnualPremiums(string accountNumber, string type) => $"AccountAnnualPremiums-{accountNumber}-{type}";
    public static string AccountCollaterals(string accountNumber) => $"AccountCollaterals-{accountNumber}";
    public static string AccountOutstandingLiability(string accountNumber) => $"AccountOutstandingLiability-{accountNumber}";
    public static string AccountWatches(string accountNumber) => $"AccountWatches-{accountNumber}";
    public static string Agency(string agencyNumber) => $"Agency-{agencyNumber}";
    public static string AgencyAgents(string id) => $"AgencyAgents-{id}";
    public static string AgencyAgentLicenses(string agencyId, string agentId) => $"AgencyAgentLicenses-{agencyId}-{agentId}";
    public static string AgencyLicenses(string id) => $"AgencyLicenses-{id}";
    public static string AccountLOAs(string accountNumber) => $"AccountLOAs-{accountNumber}";
    
    // B
    public static string BondByBlock(Guid bondBlockId) => $"BondByBlock-{bondBlockId}";
    
    // C
    public static string CreditReport(string accountNumber) => $"BondTransactions-{accountNumber}";

    // L
    public static string LegalEntityAddresses(Guid id) => $"LegalEntityAddresses-{id}";
    public static string LegalEntityEmails(Guid id) => $"LegalEntityPhoneEmails-{id}";
    public static string LegalEntityPhoneNumbers(Guid id) => $"LegalEntityPhoneNumbers-{id}";
    public static string LastIndemnitor(string accountNumber) => $"LastIndemnitor-{accountNumber}";
    public static string LastPrivateEquity(string accountNumber) => $"LastPrivateEquity-{accountNumber}";
    
    // U
    public static string UnderwriterRecommendation(string accountNum) => $"UnderwriterRecommendation-{accountNum}";
    public static string UserSettings(string userId, string? subKey)
    {
        var key = $"UserSettings-{userId.ToLower()}";
        
        if (subKey != null) 
            key += $"-{subKey}";
        
        return key;
    }
    
    // R
    public static string RelatedAccounts(string accountNumber) => $"RelatedAccounts-{accountNumber}";

    // Fixed
    // A
    public const string AddressTypes = "AddressTypes";
    public const string AgenciesDto = "AgenciesDto";
    public const string AgencyStatuses = "AgencyStatuses";
    // B
    public const string BondBlocks = "BondBlocks";
    public const string Branches = "Branches";
    public const string BusinessClasses = "BusinessClasses";
    public const string BusinessTypes = "BusinessTypes";
    // C
    public const string CommercialBondTypes = "CommercialBondTypes";
    public const string CommercialRates = "CommercialRates";
    public const string ContractRates = "ContractRates";
    public const string Countries = "Countries";
    public const string CreditReportAgencies = "CreditReportAgencies";
    // D
    public const string Divisions = "Divisions";
    // E
    public const string EmailTypes = "EmailTypes";
    // I
    public const string IndustryCodes = "IndustryCodes";
    public const string Insurers = "Insurers";
    // P
    public const string PhoneTypes = "PhoneTypes";
    // R
    public const string RateGroups = "RateGroups";
    public const string RateTypes = "RateTypes";
    public const string RiskTypes = "RiskTypes";
    public const string RiskCommission = "RiskCommission";
    // S
    public const string SecRoles = "SecRoles";
    public const string SicCodes = "SicCodes";
    public const string States = "States";
    // U
    public const string Underwriters = "Underwriters";
    // W
    public const string WatchStatuses = "WatchStatuses";
}