using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Insurer
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public bool Active { get; set; }

    public string? DefaultRateGroup { get; set; }

    public string CurrencyCountry { get; set; } = null!;

    public string? LenumPeopleSoft { get; set; }

    public virtual ICollection<AgencyLicense> AgencyLicenses { get; set; } = new List<AgencyLicense>();

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual CountryDm CurrencyCountryNavigation { get; set; } = null!;

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual ICollection<InsurerState> InsurerStates { get; set; } = new List<InsurerState>();

    public virtual ICollection<OnlineBondSystem> OnlineBondSystems { get; set; } = new List<OnlineBondSystem>();

    public virtual ICollection<PowerOfAttorney> PowerOfAttorneys { get; set; } = new List<PowerOfAttorney>();
}
