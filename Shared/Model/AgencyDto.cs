namespace James.Shared.Model;

public class AgencyDto
{
    public Guid Id { get; set; }
    public string AgencyNumber { get; set; } = "";
    public string FullName { get; set; } = "";
    public List<EmailDto> Emails { get; set; } = [];
    public List<AddressDto> Addresses { get; set; } = [];
}

public class EmailDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "";
    public string EmailAddress { get; set; } = "";
}

public class AddressDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "";
    public string Address1 { get; set; } = "";
    public string Address2 { get; set; } = "";
    public string Address3 { get; set; } = "";
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string StateCode { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Country { get; set; } = "";
    public string CountryCode { get; set; } = "";
}