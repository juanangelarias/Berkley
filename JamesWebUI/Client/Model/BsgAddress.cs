using James.Shared.Model;

namespace JamesWebUI.Client.Model;

public class BsgAddress: Address
{
    public string Type { get; set; } = string.Empty;
    public bool IsNew { get; set; }
    
    public BsgAddress()
    {
    }
    
    public BsgAddress(string type, Address address, bool isNew = false) 
    {
        Id = address.Id;
        Type = type;
        IsNew = isNew;
        Address1 = address.Address1;
        Address2 = address.Address2;
        Address3 = address.Address3;
        City = address.City;
        StateCode = address.StateCode;
        PostalCode = address.PostalCode;
    }
}