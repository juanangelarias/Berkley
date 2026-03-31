using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondBlock
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Prefix { get; set; } = null!;

    public int FirstNumber { get; set; }

    public int LastNumber { get; set; }

    public bool Enabled { get; set; }

    public bool AgencyRestricted { get; set; }

    public Guid? IssuedBy { get; set; }

    public string? Comments { get; set; }

    public Guid? AgencyId { get; set; }

    public Guid? InsuredId { get; set; }

    public virtual ICollection<BondBlockAllowedAgency> BondBlockAllowedAgencies { get; set; } = new List<BondBlockAllowedAgency>();

    public virtual Employee? IssuedByNavigation { get; set; }
}
