using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Address
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public string City { get; set; } = null!;

    public string? StateCode { get; set; }

    public string? PostalCode { get; set; }

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual LegalEntityAddress? LegalEntityAddress { get; set; }

    public virtual State? StateCodeNavigation { get; set; }
}
