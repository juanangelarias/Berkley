using James.Shared.Model;

namespace JamesWebUI.Client.Model;

public class BsgEmail: LegalEntityEmail
{
    public bool IsNew { get; set; }

    public BsgEmail()
    {
    }

    public BsgEmail(LegalEntityEmail email, bool isNew = false)
    {
        Id = email.Id;
        IsNew = isNew;
        EmailAddress = email.EmailAddress;
        Type = email.Type;
    }
}