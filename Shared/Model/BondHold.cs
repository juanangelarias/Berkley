using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondHold
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public string AccountNum { get; set; } = null!;

    public Guid? BondTypeId { get; set; }

    public string? Risk { get; set; }

    public string? Branch { get; set; }

    public Guid? UnderWriterId { get; set; }

    public Guid? AgencyId { get; set; }

    public Guid? AgentId { get; set; }

    public Guid? ObligeeId { get; set; }

    public Guid? ResponsiblePartyId { get; set; }

    public Guid? InsurerId { get; set; }

    public string? Siccode { get; set; }

    public string? State { get; set; }

    public string? Municipality { get; set; }

    public DateTime? Effective { get; set; }

    public DateTime? Expiration { get; set; }

    public DateOnly? BillDate { get; set; }

    public int? Sfaacode { get; set; }

    public int? SfaaclassCode { get; set; }

    public DateOnly? StatusLetterNext { get; set; }

    public int? ContractAmount { get; set; }

    public int? BondAmount { get; set; }

    public int? PayBondAmout { get; set; }

    public int? Premium { get; set; }

    public double? MunicipalTax { get; set; }

    public double? CommissionRate { get; set; }

    public double? RiskDescription { get; set; }

    public string? Comments { get; set; }

    public string? ExternalRemarks { get; set; }

    public string? Rate { get; set; }

    public string? RateStructure { get; set; }

    public string? RateClass { get; set; }

    public string? TerminationType { get; set; }

    public bool Cancellable { get; set; }

    public bool? RenewalProvision { get; set; }

    public int? CurrentTreatyYear { get; set; }

    public DateOnly? AccountingDate { get; set; }

    public string? BidNumber { get; set; }

    public DateTime? HomeOfficeApproved { get; set; }

    public Guid? HomeOfficeApprovedBy { get; set; }

    public string? HomeOfficeAction { get; set; }

    public bool? IsDirectBill { get; set; }

    public double? CommissionAmount { get; set; }

    public string? BondPostalCode { get; set; }

    public int? DaysToCancel { get; set; }

    public bool SurchargeApply { get; set; }

    public double? Surcharge { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual LegalEntity? Agency { get; set; }

    public virtual LegalEntity? Agent { get; set; }

    public virtual BondTypeDm? BondType { get; set; }

    public virtual UserProfile? HomeOfficeApprovedByNavigation { get; set; }

    public virtual Insurer? Insurer { get; set; }

    public virtual LegalEntity? Obligee { get; set; }

    public virtual LegalEntity? ResponsibleParty { get; set; }

    public virtual Sfaa? SfaacodeNavigation { get; set; }

    public virtual Sic? SiccodeNavigation { get; set; }

    public virtual State? StateNavigation { get; set; }

    public virtual Underwriter? UnderWriter { get; set; }
}
