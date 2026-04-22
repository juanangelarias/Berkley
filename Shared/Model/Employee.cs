using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Employee
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string ActiveDirectoryAccount { get; set; } = null!;

    public string Initials { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Title { get; set; }

    public string? Email { get; set; }

    public Guid? PrimaryBranchId { get; set; }

    public bool HomeOfficeApprover { get; set; }

    public bool Active { get; set; }

    public string? EntraId { get; set; }

    public string? ActiveDirectoryTitle { get; set; }

    public virtual ICollection<Account> AccountBranchReviewByNavigations { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountHomeOfficeReviewByNavigations { get; set; } = new List<Account>();

    public virtual ICollection<AccountProgram> AccountProgramApprovedByNavigations { get; set; } = new List<AccountProgram>();

    public virtual ICollection<AccountProgram> AccountProgramCreatedByNavigations { get; set; } = new List<AccountProgram>();

    public virtual ICollection<AccountProgramEmailNotificationGroup> AccountProgramEmailNotificationGroups { get; set; } = new List<AccountProgramEmailNotificationGroup>();

    public virtual ICollection<AccountProgramStatusHistory> AccountProgramStatusHistories { get; set; } = new List<AccountProgramStatusHistory>();

    public virtual ICollection<AccountStatusLog> AccountStatusLogs { get; set; } = new List<AccountStatusLog>();

    public virtual ICollection<AgencyCompetition> AgencyCompetitions { get; set; } = new List<AgencyCompetition>();

    public virtual ICollection<AgencyInventory> AgencyInventories { get; set; } = new List<AgencyInventory>();

    public virtual ICollection<AgencyLineOfAuthorityLog> AgencyLineOfAuthorityLogs { get; set; } = new List<AgencyLineOfAuthorityLog>();

    public virtual ICollection<AgencyStatusLog> AgencyStatusLogs { get; set; } = new List<AgencyStatusLog>();

    public virtual ICollection<BondBlock> BondBlocks { get; set; } = new List<BondBlock>();

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<BondRequest> BondRequestCctoNavigations { get; set; } = new List<BondRequest>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercialCctoNavigations { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercialHomeOfficeApprovedByNavigations { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercialRecordedByNavigations { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequest> BondRequestHomeOfficeApproverNavigations { get; set; } = new List<BondRequest>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<LineOfAuthorityLog> LineOfAuthorityLogApprovedByNavigations { get; set; } = new List<LineOfAuthorityLog>();

    public virtual ICollection<LineOfAuthorityLog> LineOfAuthorityLogCreatedByNavigations { get; set; } = new List<LineOfAuthorityLog>();

    public virtual ICollection<LineOfAuthorityReason> LineOfAuthorityReasons { get; set; } = new List<LineOfAuthorityReason>();

    public virtual ICollection<NotebookEntry> NotebookEntries { get; set; } = new List<NotebookEntry>();

    public virtual ICollection<NotificationGroupMember> NotificationGroupMembers { get; set; } = new List<NotificationGroupMember>();

    public virtual ICollection<Obligee> Obligees { get; set; } = new List<Obligee>();

    public virtual Branch? PrimaryBranch { get; set; }

    public virtual ICollection<PrivateEquity> PrivateEquities { get; set; } = new List<PrivateEquity>();

    public virtual ICollection<Surcharge> Surcharges { get; set; } = new List<Surcharge>();

    public virtual ICollection<Underwriter> UnderwriterHomeOfficeLeadNavigations { get; set; } = new List<Underwriter>();

    public virtual Underwriter? UnderwriterIdNavigation { get; set; }

    public virtual ICollection<UnderwriterRecommendation> UnderwriterRecommendations { get; set; } = new List<UnderwriterRecommendation>();

    public virtual ICollection<Underwriter> UnderwriterReportsToNavigations { get; set; } = new List<Underwriter>();

    public virtual ICollection<UserLineOfAuthority> UserLineOfAuthorityCreatedByNavigations { get; set; } = new List<UserLineOfAuthority>();

    public virtual ICollection<UserLineOfAuthority> UserLineOfAuthorityModifiedByNavigations { get; set; } = new List<UserLineOfAuthority>();

    public virtual ICollection<UserLineOfAuthority> UserLineOfAuthorityUsers { get; set; } = new List<UserLineOfAuthority>();

    public virtual ICollection<VoidedBond> VoidedBonds { get; set; } = new List<VoidedBond>();
}
