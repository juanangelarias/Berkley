using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialRatio
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid StatementId { get; set; }

    public double? CurrentStated { get; set; }

    public double? QuickStated { get; set; }

    public double? FixedAsssets2NetWorthStated { get; set; }

    public double? Gaexpenses2SalesStated { get; set; }

    public double? ArapbalanceStated { get; set; }

    public double? UnderBills2SalesStated { get; set; }

    public double? DaysInCashStated { get; set; }

    public double? AppayableDaysStated { get; set; }

    public double? ArcollectionDaysStated { get; set; }

    public double? ArcollectionDatsNoReturnStated { get; set; }

    public double? BillingTurnoverDaysStated { get; set; }

    public double? InventoryTurnoverDaysStated { get; set; }

    public double? Sales2EquityStated { get; set; }

    public double? Sales2TotalAssetsStated { get; set; }

    public double? Sales2WorkingCapitalStated { get; set; }

    public double? UnderBills2EquityStated { get; set; }

    public double? UnderBills2WorkingCapitalStated { get; set; }

    public double? TotalDebt2AssetsStated { get; set; }

    public double? TotalDebt2EquityStated { get; set; }

    public double? TotalDebt2WorkingCaptialStated { get; set; }

    public double? TermDebt2EquityStated { get; set; }

    public double? TermDebt2WorkingCapitalStated { get; set; }

    public double? GrossProfitMarginStated { get; set; }

    public double? NetProfitMarginStated { get; set; }

    public double? OperatingProfitMarginStated { get; set; }

    public double? ReturnOnAssetsStated { get; set; }

    public double? ReturnOnEquityStated { get; set; }

    public double? ReturnNetRetainedStated { get; set; }

    public double? WorkingCapital2AggregateProgramStated { get; set; }

    public double? CurrentAllowed { get; set; }

    public double? QuickAllowed { get; set; }

    public double? FixedAsssets2NetWorthAllowed { get; set; }

    public double? Gaexpenses2SalesAllowed { get; set; }

    public double? ArapbalanceAllowed { get; set; }

    public double? UnderBills2SalesAllowed { get; set; }

    public double? DaysInCashAllowed { get; set; }

    public double? AppayableDaysAllowed { get; set; }

    public double? ArcollectionDaysAllowed { get; set; }

    public double? ArcollectionDatsNoReturnAllowed { get; set; }

    public double? BillingTurnoverDaysAllowed { get; set; }

    public double? InventoryTurnoverDaysAllowed { get; set; }

    public double? Sales2EquityAllowed { get; set; }

    public double? Sales2TotalAssetsAllowed { get; set; }

    public double? Sales2WorkingCapitalAllowed { get; set; }

    public double? UnderBills2EquityAllowed { get; set; }

    public double? UnderBills2WorkingCapitalAllowed { get; set; }

    public double? TotalDebt2AssetsAllowed { get; set; }

    public double? TotalDebt2EquityAllowed { get; set; }

    public double? TotalDebt2WorkingCaptialAllowed { get; set; }

    public double? TermDebt2EquityAllowed { get; set; }

    public double? TermDebt2WorkingCapitalAllowed { get; set; }

    public double? GrossProfitMarginAllowed { get; set; }

    public double? NetProfitMarginAllowed { get; set; }

    public double? OperatingProfitMarginAllowed { get; set; }

    public double? ReturnOnAssetsAllowed { get; set; }

    public double? ReturnOnEquityAllowed { get; set; }

    public double? ReturnNetRetainedAllowed { get; set; }

    public double? WorkingCapital2AggregateProgramAllowed { get; set; }

    public double? SingleProgram2LargestJob { get; set; }

    public double? AggregateProgram2Sales { get; set; }

    public double? AggregateProgram2LargestBacklog { get; set; }

    public virtual FinancialStatement Statement { get; set; } = null!;
}
