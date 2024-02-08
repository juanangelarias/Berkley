using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LegalEntity
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid Parent { get; set; }

    public string FullName { get; set; } = null!;

    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }

    public string? MiddleInitial { get; set; }

    public string EntityType { get; set; } = null!;

    public string? Website { get; set; }

    public bool IsIndividual { get; set; }

    public string? TaxIdEncrypted { get; set; }

    public virtual ICollection<Account> AccountCpacontacts { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountCpafirms { get; set; } = new List<Account>();

    public virtual Account? AccountIdNavigation { get; set; }

    public virtual AccountReference? AccountReference { get; set; }

    public virtual AdditionalRelatedParty? AdditionalRelatedParty { get; set; }

    public virtual ICollection<Agency> AgencyBillingContacts { get; set; } = new List<Agency>();

    public virtual Agency? AgencyIdNavigation { get; set; }

    public virtual ICollection<AgencyInventory> AgencyInventories { get; set; } = new List<AgencyInventory>();

    public virtual ICollection<AgencyLicense> AgencyLicenseAgents { get; set; } = new List<AgencyLicense>();

    public virtual AgencyLicense? AgencyLicenseIdNavigation { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual ICollection<Bond> BondAgencies { get; set; } = new List<Bond>();

    public virtual ICollection<Bond> BondAgents { get; set; } = new List<Bond>();

    public virtual ICollection<BondHold> BondHoldAgencies { get; set; } = new List<BondHold>();

    public virtual ICollection<BondHold> BondHoldAgents { get; set; } = new List<BondHold>();

    public virtual ICollection<BondHold> BondHoldObligees { get; set; } = new List<BondHold>();

    public virtual ICollection<BondHold> BondHoldResponsibleParties { get; set; } = new List<BondHold>();

    public virtual ICollection<Bond> BondObligees { get; set; } = new List<Bond>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercials { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<Bond> BondResponsibleParties { get; set; } = new List<Bond>();

    public virtual LegalEntityTypeDm EntityTypeNavigation { get; set; } = null!;

    public virtual Indemnitor? Indemnitor { get; set; }

    public virtual Insurer? Insurer { get; set; }

    public virtual ICollection<LegalEntity> InverseParentNavigation { get; set; } = new List<LegalEntity>();

    public virtual KeyPersonel? KeyPersonel { get; set; }

    public virtual LawEntity? LawEntity { get; set; }

    public virtual ICollection<LegalEntityAddress> LegalEntityAddresses { get; set; } = new List<LegalEntityAddress>();

    public virtual ICollection<LegalEntityEmail> LegalEntityEmails { get; set; } = new List<LegalEntityEmail>();

    public virtual ICollection<LegalEntityPhone> LegalEntityPhones { get; set; } = new List<LegalEntityPhone>();

    public virtual Obligee? Obligee { get; set; }

    public virtual LegalEntity? ParentNavigation { get; set; } = null;

    public virtual ICollection<PersonalFinancialHeader> PersonalFinancialHeaders { get; set; } = new List<PersonalFinancialHeader>();
}
