using James.Shared.Model;

namespace SharedBusinessLogic;

public static class LegalEntityGetMains
{
    public static string? GetMainEmail(LegalEntity entity)
    {
        return entity.LegalEntityEmails
            .FirstOrDefault(f => f.Type.ToLower() == "main")?
            .EmailAddress;
    }
    
    public static PhoneNumber? GetMainPhoneNumber(LegalEntity entity)
    {
        return entity.LegalEntityPhones
            .FirstOrDefault(f => f.Type.ToLower() == "main")?
            .PhoneNumber;
    }
    
    public static Address? GetMainAddress(LegalEntity entity)
    {
        return entity.LegalEntityAddresses
            .FirstOrDefault(f => f.Type.ToLower() == "main")?
            .Address;
    }
}