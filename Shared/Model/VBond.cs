using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class VBond
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public int CurrentBondMod { get; set; }

    public string AccountNum { get; set; } = null!;

    public string BondType { get; set; } = null!;

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public string Status { get; set; } = null!;

    public Guid UnderWriterId { get; set; }

    public Guid? AgentId { get; set; }

    public string? RenewalProvision { get; set; }

    public bool? Cancellable { get; set; }

    public Guid InsurerId { get; set; }

    public string State { get; set; } = null!;

    public string? RiskDescription { get; set; }

    public string? Comments { get; set; }

    public string? TerminationType { get; set; }

    public int? SfaaclassCode { get; set; }

    public string? Siccode { get; set; }

    public bool AdditionalObligees { get; set; }

    public int? CurrentTreatyYear { get; set; }

    public string? Municipality { get; set; }

    public string BondClass { get; set; } = null!;

    public string? Risk { get; set; }

    public string? ShortRiskDescription { get; set; }

    public bool JointVenture { get; set; }

    public Guid? ObligeeId { get; set; }

    public Guid? ResponsiblePartyId { get; set; }

    public string RateGroup { get; set; } = null!;

    public double? RateMultiplier { get; set; }

    public string? CeritifiedMailNumber { get; set; }

    public DateTime? NonrenewalLetterMailed { get; set; }

    public bool HasCollateral { get; set; }

    public bool SubjectToRunoff { get; set; }

    public bool? Federal { get; set; }

    public DateTime? HomeOfficeApproved { get; set; }

    public string? HomeOfficeApprovedBy { get; set; }

    public string? HomeOfficeAction { get; set; }

    public DateTime? LineOfAuthorityExpiration { get; set; }

    public int? LineOfAuthoritySingle { get; set; }

    public int? LineOfAuthorityAggregate { get; set; }

    public bool? LineOfAuthorityException { get; set; }

    public string? LineOfAuthorityExceptionDescription { get; set; }

    public int CurrentBondLiability { get; set; }

    public int CurrentRunoff { get; set; }

    public int? EstimatedBondLiability { get; set; }

    public bool Claim { get; set; }

    public Guid? AttorneyInFactId { get; set; }

    public bool? IsDirectBill { get; set; }

    public Guid? DirectBillAddressId { get; set; }

    public string? BondPostalCode { get; set; }

    public int? DaysToCancel { get; set; }

    public string? ExternalRemarks { get; set; }

    public string? SfaabondType { get; set; }

    public string AgencyNumber { get; set; } = null!;

    public string AgencyName { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public int BondAmount { get; set; }

    public int SfaaCode { get; set; }

    public bool NmlsclassCode { get; set; }

    public string? RateClass { get; set; }

    public DateTime CurrentTermEffective { get; set; }

    public DateTime CurrentTermExpiration { get; set; }
}
