using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UserProfile
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Initials { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Title { get; set; }

    public Guid? PrimaryBranchId { get; set; }

    public string Username { get; set; } = null!;

    public bool Underwriter { get; set; }

    public bool HomeOfficeApprover { get; set; }

    public virtual ICollection<Account> AccountBranchReviewByNavigations { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountHomeOfficeReviewByNavigations { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountUnderwriterNavigations { get; set; } = new List<Account>();

    public virtual ICollection<AgencyCompetition> AgencyCompetitions { get; set; } = new List<AgencyCompetition>();

    public virtual ICollection<AgencyInventory> AgencyInventories { get; set; } = new List<AgencyInventory>();

    public virtual ICollection<AgencyStatusLog> AgencyStatusLogs { get; set; } = new List<AgencyStatusLog>();

    public virtual ICollection<BidRequest> BidRequestCctoNavigations { get; set; } = new List<BidRequest>();

    public virtual ICollection<BidRequestCommercial> BidRequestCommercialCctoNavigations { get; set; } = new List<BidRequestCommercial>();

    public virtual ICollection<BidRequestCommercial> BidRequestCommercialHomeOfficeApprovedByNavigations { get; set; } = new List<BidRequestCommercial>();

    public virtual ICollection<BidRequestCommercial> BidRequestCommercialRecordedByNavigations { get; set; } = new List<BidRequestCommercial>();

    public virtual ICollection<BidRequestCommercial> BidRequestCommercialUnderwriterNavigations { get; set; } = new List<BidRequestCommercial>();

    public virtual ICollection<BidRequest> BidRequestUnderwriterNavigations { get; set; } = new List<BidRequest>();

    public virtual ICollection<BondBlock> BondBlocks { get; set; } = new List<BondBlock>();

    public virtual ICollection<BondHold> BondHoldHomeOfficeApprovedByNavigations { get; set; } = new List<BondHold>();

    public virtual ICollection<BondHold> BondHoldUnderwriterNavigations { get; set; } = new List<BondHold>();

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual ICollection<LineOauthorityLog> LineOauthorityLogApprovedByNavigations { get; set; } = new List<LineOauthorityLog>();

    public virtual ICollection<LineOauthorityLog> LineOauthorityLogCreatedByNavigations { get; set; } = new List<LineOauthorityLog>();

    public virtual ICollection<NotebookEntry> NotebookEntries { get; set; } = new List<NotebookEntry>();

    public virtual ICollection<ProfitCenter> ProfitCenters { get; set; } = new List<ProfitCenter>();

    public virtual ICollection<Surcharge> Surcharges { get; set; } = new List<Surcharge>();

    public virtual ICollection<UserLineOfAuthority> UserLineOfAuthorityCreatedByNavigations { get; set; } = new List<UserLineOfAuthority>();

    public virtual ICollection<UserLineOfAuthority> UserLineOfAuthorityModifiedByNavigations { get; set; } = new List<UserLineOfAuthority>();

    public virtual ICollection<UserLineOfAuthority> UserLineOfAuthorityUsers { get; set; } = new List<UserLineOfAuthority>();

    public virtual ICollection<VoidedBond> VoidedBonds { get; set; } = new List<VoidedBond>();

    public virtual ICollection<AccountStatusLog> AccountStatusLogs { get; set; }=new List<AccountStatusLog>();
}
