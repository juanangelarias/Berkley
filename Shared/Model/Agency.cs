using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Agency
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AgencyNumber { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Comments { get; set; }

    public bool Nasbp { get; set; }

    public bool W9 { get; set; }

    public bool ProfitSharing { get; set; }

    public int? ProfitSharingMinimumPremium { get; set; }

    public Guid? BillingContactId { get; set; }

    public string? NationalProducerNumber { get; set; }

    public string? ErrorsAndOmmissionsCarrier { get; set; }

    public DateTime? ErrorsAndOmmissionsExpiration { get; set; }

    public int? ErrorsAndOmmissionsCoverage { get; set; }

    public bool Need1099 { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<AgencyCompetition> AgencyCompetitions { get; set; } = new List<AgencyCompetition>();

    public virtual ICollection<AgencyErrorAndOmission> AgencyErrorAndOmissions { get; set; } = new List<AgencyErrorAndOmission>();

    public virtual ICollection<AgencyInventory> AgencyInventories { get; set; } = new List<AgencyInventory>();

    public virtual ICollection<AgencyLicense> AgencyLicenses { get; set; } = new List<AgencyLicense>();

    public virtual ICollection<AgencyStatusLog> AgencyStatusLogs { get; set; } = new List<AgencyStatusLog>();

    public virtual ICollection<AgentsInAgency> AgentsInAgencies { get; set; } = new List<AgentsInAgency>();

    public virtual LegalEntity? BillingContact { get; set; }

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual ICollection<PowerOfAttorney> PowerOfAttorneys { get; set; } = new List<PowerOfAttorney>();

    public virtual AgencyStatusDm StatusNavigation { get; set; } = null!;

    public virtual ICollection<VoidedBond> VoidedBonds { get; set; } = new List<VoidedBond>();
}
