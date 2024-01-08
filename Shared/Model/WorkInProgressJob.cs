using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class WorkInProgressJob
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly Wipdate { get; set; }

    public string JobNumber { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? BondNumber { get; set; }

    public long ContractPrice { get; set; }

    public long EstimatedCost { get; set; }

    public long EstimatedGrossProfit { get; set; }

    public double PercentComplete { get; set; }

    public long EarnedRevenue { get; set; }

    public long CostToDate { get; set; }

    public long GrossProfit { get; set; }

    public decimal? GrossProfitPercent { get; set; }

    public long? ProgressBillings { get; set; }

    public long UnderBillings { get; set; }

    public long OverBillings { get; set; }

    public long RevenueRemaining { get; set; }

    public long CostToComplete { get; set; }

    public long TotalEstimatedGrossProfit { get; set; }

    public string JobStatus { get; set; } = null!;

    public long? EnteredContractPrice { get; set; }

    public long? EnteredEstimatedCost { get; set; }

    public long? EnteredCostToDate { get; set; }

    public long? EnteredProgressBillings { get; set; }

    public long? OtherJobsContractPrice { get; set; }

    public long? OtherJobsEstimatedCost { get; set; }

    public long? OtherJobsCostToDate { get; set; }

    public long? OtherJobsProgressBillings { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Bond? BondNumberNavigation { get; set; }

    public virtual WorkInProgressSummary WorkInProgressSummary { get; set; } = null!;
}
