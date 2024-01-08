using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialRatio
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly StatementDate { get; set; }

    public double? CurrentStated { get; set; }

    public double? QuickStated { get; set; }

    public double? FixedAsssets2NetWorthStated { get; set; }

    public double? Gaexpenses2SalesStated { get; set; }

    public double? ArapbalanceStated { get; set; }

    public double? UnderBills2SalesStated { get; set; }

    public double? DaysInCashStated { get; set; }

    public double? CurrentAllowed { get; set; }

    public double? QuickAllowed { get; set; }

    public double? WorkingCapital2AggregateProgram { get; set; }

    public double? FixedAsssets2NetWorthAllowed { get; set; }

    public double? Gaexpenses2SalesAllowed { get; set; }

    public double? ArapbalanceAllowed { get; set; }

    public double? UnderBills2SalesAllowed { get; set; }

    public double? DaysInCashAllowed { get; set; }

    public double? AppayableDays { get; set; }

    public double? ArcollectionDays { get; set; }

    public double? ArcollectionDatsNoReturn { get; set; }

    public double? BillingTurnoverDays { get; set; }

    public double? InventoryTurnoverDays { get; set; }

    public double? Sales2Equity { get; set; }

    public double? Sales2TotalAssets { get; set; }

    public double? Sales2WorkingCapital { get; set; }

    public double? UnderBills2Equity { get; set; }

    public double? UnderBills2WorkingCapital { get; set; }

    public double? TermDebt2Equity { get; set; }

    public double? TermDebt2WorkingCapital { get; set; }

    public double? GrossProfitMargin { get; set; }

    public double? NetProfitMargin { get; set; }

    public double? OperatingProfitMargin { get; set; }

    public double? ReturnOnAssets { get; set; }

    public double? ReturnOnEquity { get; set; }

    public double? ReturnNetRetained { get; set; }

    public double? SingloProgram2LargestJob { get; set; }

    public double? AggregateProgram2LargetBacking { get; set; }

    public double? AggregateProgram2Sales { get; set; }

    public double? AggregateProgram2LargetBacklog { get; set; }
}
