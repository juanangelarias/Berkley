using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class WorkInProgressSummary
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly Wipdate { get; set; }

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

    public virtual ICollection<WorkInProgressJob> WorkInProgressJobs { get; set; } = new List<WorkInProgressJob>();
}
