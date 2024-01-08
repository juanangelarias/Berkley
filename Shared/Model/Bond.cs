using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Bond
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public int CurrentBondMod { get; set; }

    public string AccountNum { get; set; } = null!;

    public Guid? BondTypeId { get; set; }

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public string Status { get; set; } = null!;

    public Guid UnderWriterId { get; set; }

    public Guid? AgentId { get; set; }

    public string? RenewalProvision { get; set; }

    public bool? Cancellable { get; set; }

    public Guid InsurerId { get; set; }

    public Guid AgencyId { get; set; }

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

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual ICollection<AdditionalObligee> AdditionalObligeesNavigation { get; set; } = new List<AdditionalObligee>();

    public virtual LegalEntity Agency { get; set; } = null!;

    public virtual LegalEntity? Agent { get; set; }

    public virtual LawEntity? AttorneyInFact { get; set; }

    public virtual ICollection<BondRequestCommercial> BondRequestCommercials { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequest> BondRequests { get; set; } = new List<BondRequest>();

    public virtual ICollection<BondStatusLetter> BondStatusLetters { get; set; } = new List<BondStatusLetter>();

    public virtual ICollection<BondTransactionPurpose> BondTransactionPurposes { get; set; } = new List<BondTransactionPurpose>();

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual BondTypeDm? BondType { get; set; }

    public virtual ICollection<CoPrincipal> CoPrincipals { get; set; } = new List<CoPrincipal>();

    public virtual ICollection<Collateral> Collaterals { get; set; } = new List<Collateral>();

    public virtual Address? DirectBillAddress { get; set; }

    public virtual ICollection<EmailHistory> EmailHistories { get; set; } = new List<EmailHistory>();

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual LegalEntity? Obligee { get; set; }

    public virtual ICollection<OpenClaim> OpenClaims { get; set; } = new List<OpenClaim>();

    public virtual LegalEntity? ResponsibleParty { get; set; }

    public virtual SfaabondTypeDm? SfaabondTypeNavigation { get; set; }

    public virtual Sic? SiccodeNavigation { get; set; }

    public virtual State StateNavigation { get; set; } = null!;

    public virtual Underwriter UnderWriter { get; set; } = null!;

    public virtual ICollection<WorkInProgressJob> WorkInProgressJobs { get; set; } = new List<WorkInProgressJob>();
}
