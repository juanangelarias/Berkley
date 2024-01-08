using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Account
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public bool OpenClaim { get; set; }

    public string? YearOpened { get; set; }

    public string Branch { get; set; } = null!;

    public string? AgencyNumber { get; set; }

    public string? DunBradstreetRate { get; set; }

    public DateOnly? DunBradstreetDate { get; set; }

    public string? DunBradstreetSic { get; set; }

    public string? Bank { get; set; }

    public string? BankReferenceName { get; set; }

    public int? BankAverageBalance { get; set; }

    public Guid? BankPhoneId { get; set; }

    public DateOnly? BankLastContacted { get; set; }

    public int? BankLoc { get; set; }

    public DateOnly? BankLocexpires { get; set; }

    public int? BankLocused { get; set; }

    public int? BankLochigh { get; set; }

    public DateOnly? BankLochighDate { get; set; }

    public string? BankLocsecurity { get; set; }

    public double? PublicPercent { get; set; }

    public double? PrivatePercent { get; set; }

    public double? SubcontractPercent { get; set; }

    public string? SubcontractProtection { get; set; }

    public string? GeographicSpread { get; set; }

    public int? PaydexIntelliscore { get; set; }

    public string? PriorSuretyCompany { get; set; }

    public string? CurrentManagementYear { get; set; }

    public bool IndemnityFull { get; set; }

    public bool IndemnityCorp { get; set; }

    public bool IndemnityPerson { get; set; }

    public bool BerkleyAffiliate { get; set; }

    public bool ContinuityKeyManagementLifeInsurance { get; set; }

    public bool ContinuityManagementIncentives { get; set; }

    public bool ContinuityFundedBuySell { get; set; }

    public bool ContinuityActiveMultipleOwners { get; set; }

    public string? FiscalYearEnd { get; set; }

    public string? TaxBasis { get; set; }

    public Guid? AgentId { get; set; }

    public string? BusinessType { get; set; }

    public string Division { get; set; } = null!;

    public double RateModifier { get; set; }

    public Guid? LawFirmId { get; set; }

    public Guid? AttorneyId { get; set; }

    public Guid? CpafirmId { get; set; }

    public Guid? CpacontactId { get; set; }

    public Guid? UnderwriterId { get; set; }

    public string WatchStatus { get; set; } = null!;

    public string? Naics { get; set; }

    public Guid? HomeOfficeReviewBy { get; set; }

    public DateTime? HomeOfficeReviewed { get; set; }

    public Guid? BranchReviewBy { get; set; }

    public DateTime? BranchReviewed { get; set; }

    public bool HomeOfficeLock { get; set; }

    public string? BusinessTypeClass { get; set; }

    public bool FundsControl { get; set; }

    public string? PolutionLiabilityCarrier { get; set; }

    public DateOnly? PolutionLiabilityExpires { get; set; }

    public bool AffiliateCompany { get; set; }

    public string? CreditReport { get; set; }

    public string? EstimatingSystem { get; set; }

    public string? EstimatingSignoff { get; set; }

    public string? AccountingSystem { get; set; }

    public bool Pocinterims { get; set; }

    public bool InterimWips { get; set; }

    public virtual ICollection<AccountProgram> AccountPrograms { get; set; } = new List<AccountProgram>();

    public virtual ICollection<AdditionalRelatedParty> AdditionalRelatedParties { get; set; } = new List<AdditionalRelatedParty>();

    public virtual ICollection<AgencyCompetition> AgencyCompetitions { get; set; } = new List<AgencyCompetition>();

    public virtual Agency? AgencyNumberNavigation { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual LawEntity? Attorney { get; set; }

    public virtual ICollection<BalanceSheet> BalanceSheets { get; set; } = new List<BalanceSheet>();

    public virtual PhoneNumber? BankPhone { get; set; }

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercials { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequest> BondRequests { get; set; } = new List<BondRequest>();

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual Branch BranchNavigation { get; set; } = null!;

    public virtual UserProfile? BranchReviewByNavigation { get; set; }

    public virtual BusinessTypeClassCodeDm? BusinessTypeClassNavigation { get; set; }

    public virtual BusinessTypeDm? BusinessTypeNavigation { get; set; }

    public virtual ICollection<CashFlowStatement> CashFlowStatements { get; set; } = new List<CashFlowStatement>();

    public virtual ICollection<CoPrincipal> CoPrincipals { get; set; } = new List<CoPrincipal>();

    public virtual LegalEntity? Cpacontact { get; set; }

    public virtual LegalEntity? Cpafirm { get; set; }

    public virtual DivisionDm DivisionNavigation { get; set; } = null!;

    public virtual ICollection<EmailHistory> EmailHistories { get; set; } = new List<EmailHistory>();

    public virtual UserProfile? HomeOfficeReviewByNavigation { get; set; }

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual ICollection<Indemnitor> Indemnitors { get; set; } = new List<Indemnitor>();

    public virtual ICollection<KeyPersonel> KeyPersonels { get; set; } = new List<KeyPersonel>();

    public virtual LawEntity? LawFirm { get; set; }

    public virtual ICollection<PersonalFinancialStatement> PersonalFinancialStatements { get; set; } = new List<PersonalFinancialStatement>();

    public virtual ICollection<Ratio> Ratios { get; set; } = new List<Ratio>();

    public virtual TaxBasisDm? TaxBasisNavigation { get; set; }

    public virtual Underwriter? Underwriter { get; set; }

    public virtual ICollection<UnderwriterRecommendation> UnderwriterRecommendations { get; set; } = new List<UnderwriterRecommendation>();

    public virtual WatchStatusDm WatchStatusNavigation { get; set; } = null!;

    public virtual ICollection<WorkInProgressJob> WorkInProgressJobs { get; set; } = new List<WorkInProgressJob>();
}
