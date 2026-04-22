using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialAggregate
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid StatementId { get; set; }

    public long? WorkingCapitalStated { get; set; }

    public long? TotalAssetsStated { get; set; }

    public long? RetainedEarningsStated { get; set; }

    public long? EarningsBeforeTaxesStated { get; set; }

    public long? NetWorthStated { get; set; }

    public long? TotalLiabilitiesStated { get; set; }

    public long? TotalEquityStated { get; set; }

    public long? TotalRevenueStated { get; set; }

    public long? TotalCostOfGoodsSoldStated { get; set; }

    public long? CurrentAssetsStated { get; set; }

    public long? CurrentLiabilitiesStated { get; set; }

    public long? NetIncomeStated { get; set; }

    public long? InventoryStated { get; set; }

    public long? FixedAssetsStated { get; set; }

    public long? AccountsReceivableStated { get; set; }

    public long? AccountsPayableStated { get; set; }

    public long? SubordinatedNotesStated { get; set; }

    public long? RetainageReceivedStated { get; set; }

    public long? RetainagePayableStated { get; set; }

    public long? CashPlusEarningsGreaterThanBillingsStated { get; set; }

    public long? BillingsGreaterThanCostsPlusEarningsStated { get; set; }

    public long? PrepaidExpensesStated { get; set; }

    public long? CashInventoryStated { get; set; }

    public long? OtherCurrentAssetsStated { get; set; }

    public long? BankDebtStated { get; set; }

    public long? GaexpensesStated { get; set; }

    public long? OperatingProfitStated { get; set; }

    public long? IncomeAfterTaxesStated { get; set; }

    public long? CashStated { get; set; }

    public long? NotesStated { get; set; }

    public long? QscoreStated { get; set; }

    public long? QprofitStated { get; set; }

    public long? QnetworthStated { get; set; }

    public decimal? QleverageStated { get; set; }

    public long? QworkingCapitalStated { get; set; }

    public decimal? QcratioStated { get; set; }

    public long? WorkingCapitalAllowed { get; set; }

    public long? TotalAssetsAllowed { get; set; }

    public long? RetainedEarningsAllowed { get; set; }

    public long? EarningsBeforeTaxesAllowed { get; set; }

    public long? NetWorthAllowed { get; set; }

    public long? TotalLiabilitiesAllowed { get; set; }

    public long? TotalEquityAllowed { get; set; }

    public long? TotalRevenueAllowed { get; set; }

    public long? TotalCostOfGoodsSoldAllowed { get; set; }

    public long? CurrentAssetsAllowed { get; set; }

    public long? CurrentLiabilitiesAllowed { get; set; }

    public long? NetIncomeAllowed { get; set; }

    public long? InventoryAllowed { get; set; }

    public long? FixedAssetsAllowed { get; set; }

    public long? AccountsReceivableAllowed { get; set; }

    public long? AccountsPayableAllowed { get; set; }

    public long? SubordinatedNotesAllowed { get; set; }

    public long? RetainageReceivedAllowed { get; set; }

    public long? RetainagePayableAllowed { get; set; }

    public long? CashPlusEarningsGreaterThanBillingsAllowed { get; set; }

    public long? BillingsGreaterThanCostsPlusEarningsAllowed { get; set; }

    public long? PrepaidExpensesAllowed { get; set; }

    public long? CashInventoryAllowed { get; set; }

    public long? OtherCurrentAssetsAllowed { get; set; }

    public long? BankDebtAllowed { get; set; }

    public long? GaexpensesAllowed { get; set; }

    public long? OperatingProfitAllowed { get; set; }

    public long? IncomeAfterTaxesAllowed { get; set; }

    public long? CashAllowed { get; set; }

    public long? NotesAllowed { get; set; }

    public long? QscoreAllowed { get; set; }

    public long? QprofitAllowed { get; set; }

    public long? QnetworthAllowed { get; set; }

    public decimal? QleverageAllowed { get; set; }

    public long? QworkingCapitalAllowed { get; set; }

    public decimal? QcratioAllowed { get; set; }

    public long? SingleLoa { get; set; }

    public long? AggregateLoa { get; set; }

    public long? LargestJob { get; set; }

    public long? LargestBacklog { get; set; }

    public long? CashFromOperations { get; set; }

    public long? CashFromInvestments { get; set; }

    public long? CashFromFinancing { get; set; }

    public string? Comments { get; set; }

    public long? NetChangeCash { get; set; }

    public virtual FinancialStatement Statement { get; set; } = null!;
}
