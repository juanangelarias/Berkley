using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class State
{
    public string Code { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public string? Saacode { get; set; }

    public string? WrbstateCode { get; set; }

    public string? CountryCode { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<AgencyLicense> AgencyLicenses { get; set; } = new List<AgencyLicense>();

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual CountryDm? CountryCodeNavigation { get; set; }

    public virtual ICollection<InsurerState> InsurerStates { get; set; } = new List<InsurerState>();

    public virtual ICollection<Surcharge> Surcharges { get; set; } = new List<Surcharge>();
}
