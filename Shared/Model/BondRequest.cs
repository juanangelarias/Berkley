using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondRequest
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondRequestNumber { get; set; } = null!;

    public string AccountNum { get; set; } = null!;

    public DateOnly Requested { get; set; }

    public DateOnly? BidDate { get; set; }

    public Guid? ObligeeId { get; set; }

    public DateOnly? EstimatedStart { get; set; }

    public DateOnly? EstimatedFinish { get; set; }

    public string? ContractNumber { get; set; }

    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    public int? EstimatedContractPrice { get; set; }

    public string? WithdrawalPenalty { get; set; }

    public int? PerformanceBondAmount { get; set; }

    public int? PaymentBondAmount { get; set; }

    public string? MaintenanceTerm { get; set; }

    public string? Penalties { get; set; }

    public string? PaymentFrequency { get; set; }

    public string? Retainage { get; set; }

    public string? SpecialHazards { get; set; }

    public string? GeographicConcerns { get; set; }

    public string? Comments { get; set; }

    public string? Conditions { get; set; }

    public long? WipworkOnHand { get; set; }

    public DateOnly? Wipdate { get; set; }

    public int? NewContractsLowBids { get; set; }

    public int? EstimatedBacklogRunoff { get; set; }

    public long? TotalWorkOnHand { get; set; }

    public int? LineOfCreditSingle { get; set; }

    public int? LineOfCreditAggregate { get; set; }

    public DateOnly? LineOfCreditExpiration { get; set; }

    public string? RequestedBy { get; set; }

    public string? ApprovedBy { get; set; }

    public string? BondNumber { get; set; }

    public int? LaborPercentage { get; set; }

    public int? MaterialPercentage { get; set; }

    public int? SubcontractedPercentage { get; set; }

    public int? GeneralConditionsPercentage { get; set; }

    public int? EstimateGrossProfitPercentage { get; set; }

    public int? EquipmentPercentage { get; set; }

    public string? BidResult { get; set; }

    public DateTime? HomeOfficeApproved { get; set; }

    public Guid? HomeOfficeApprover { get; set; }

    public DateTime? HomeOfficeEmailSent { get; set; }

    public string? HomeOfficeAction { get; set; }

    public Guid? UnderWriterId { get; set; }

    public string RequestType { get; set; } = null!;

    public int? OutstandingBids { get; set; }

    public int? Adjustments { get; set; }

    public int? ActualBidAmount { get; set; }

    public int? ApprovedBidAmount { get; set; }

    public Guid? Ccto { get; set; }

    public string Status { get; set; } = null!;

    public string? HomeOfficeConditions { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual BidResultDm? BidResultNavigation { get; set; }

    public virtual ICollection<BidSubcontractor> BidSubcontractors { get; set; } = new List<BidSubcontractor>();

    public virtual Bond? BondNumberNavigation { get; set; }

    public virtual UserProfile? CctoNavigation { get; set; }

    public virtual UserProfile? HomeOfficeApproverNavigation { get; set; }

    public virtual Obligee? Obligee { get; set; }

    public virtual ICollection<OtherBid> OtherBids { get; set; } = new List<OtherBid>();

    public virtual BidStatusDm StatusNavigation { get; set; } = null!;

    public virtual Underwriter? UnderWriter { get; set; }
}
