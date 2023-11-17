using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BidRequestCommercial
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BidNumber { get; set; } = null!;

    public string AccountNum { get; set; } = null!;

    public DateTime Requested { get; set; }

    public int? LegacyBidNumber { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? EstimatedStart { get; set; }

    public DateTime? EstimatedFinish { get; set; }

    public Guid? ObligeeId { get; set; }

    public int? Amount { get; set; }

    public string? RecordedBy { get; set; }

    public DateTime? Recorded { get; set; }

    public DateTime? RecordedMessageSent { get; set; }

    public string? HomeOfficeAction { get; set; }

    public DateTime? HomeOfficeApproved { get; set; }

    public string? HomeOfficeApprovedBy { get; set; }

    public string? HomeOfficeConditions { get; set; }

    public string? BondNumber { get; set; }

    public string? Description { get; set; }

    public string? Underwriter { get; set; }

    public int? SfaaCode { get; set; }

    public string? Ccto { get; set; }

    public int? LineOfCreditSingle { get; set; }

    public int? LineOfCreditAggregate { get; set; }

    public DateTime? LineOfCreditExpiration { get; set; }

    public int? Exposure { get; set; }

    public int? Adjustments { get; set; }

    public int? TotalExposure { get; set; }

    public int? ApprovedBidAmount { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Bond? BondNumberNavigation { get; set; }

    public virtual UserProfile? CctoNavigation { get; set; }

    public virtual UserProfile? HomeOfficeApprovedByNavigation { get; set; }

    public virtual LegalEntity? Obligee { get; set; }

    public virtual UserProfile? RecordedByNavigation { get; set; }

    public virtual Sfaa? SfaaclassCodeNavigation { get; set; }

    public virtual BidStatusDm StatusNavigation { get; set; } = null!;

    public virtual UserProfile? UnderwriterNavigation { get; set; }
}
