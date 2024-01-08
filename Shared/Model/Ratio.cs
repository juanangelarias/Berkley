using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Ratio
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly StatementDate { get; set; }

    public string? StatementType { get; set; }

    public string? StatementBasis { get; set; }

    public string? StatementTaxBasis { get; set; }

    public string? StatementQuality { get; set; }

    public int? WorkingCapitalStated { get; set; }

    public int? TotalAssetsStated { get; set; }

    public int? RetainedEarningsStated { get; set; }

    public int? EarningsBeforeTaxesStated { get; set; }

    public int? NetWorthStated { get; set; }

    public int? TotalLiabilitiesStated { get; set; }

    public int? TotalEquityStated { get; set; }

    public int? TotalRevenueStated { get; set; }

    public int? TotalCostOfGoodsSoldStated { get; set; }

    public int? CurrentAssetsStated { get; set; }

    public int? CurrentLiabilitiesStated { get; set; }

    public int? NetIncomeStated { get; set; }

    public int? InventoryStated { get; set; }

    public int? FixedAssetsStated { get; set; }

    public int? AccountsReceivableStated { get; set; }

    public int? AccountsPayableStated { get; set; }

    public int? SubordinatedNotesStated { get; set; }

    public int? RetainageReceivedStated { get; set; }

    public int? RetainagePayableStated { get; set; }

    public int? CashPlusEarningsGreaterThanBillingsStated { get; set; }

    public int? BillingsGreaterThanCostsPlusEarningsStated { get; set; }

    public int? PrepaidExpensesStated { get; set; }

    public int? CashInventoryStated { get; set; }

    public int? OtherCurrentAssetsStated { get; set; }

    public int? BankDebtStated { get; set; }

    public int? GaexpensesStated { get; set; }

    public int? OperatingProfitStated { get; set; }

    public int? IncomeAfterTaxesStated { get; set; }

    public int? CashStated { get; set; }

    public int? NotesStated { get; set; }

    public int? QscoreStated { get; set; }

    public int? QprofitStated { get; set; }

    public int? QnetworthStated { get; set; }

    public int? QleverageStated { get; set; }

    public int? QworkingCapitalStated { get; set; }

    public int? QcratioStated { get; set; }

    public int? WorkingCapitalAllowed { get; set; }

    public int? TotalAssetsAllowed { get; set; }

    public int? RetainedEarningsAllowed { get; set; }

    public int? EarningsBeforeTaxesAllowed { get; set; }

    public int? NetWorthAllowed { get; set; }

    public int? TotalLiabilitiesAllowed { get; set; }

    public int? TotalEquityAllowed { get; set; }

    public int? TotalRevenueAllowed { get; set; }

    public int? TotalCostOfGoodsSoldAllowed { get; set; }

    public int? CurrentAssetsAllowed { get; set; }

    public int? CurrentLiabilitiesAllowed { get; set; }

    public int? NetIncomeAllowed { get; set; }

    public int? InventoryAllowed { get; set; }

    public int? FixedAssetsAllowed { get; set; }

    public int? AccountsReceivableAllowed { get; set; }

    public int? AccountsPayableAllowed { get; set; }

    public int? SubordinatedNotesAllowed { get; set; }

    public int? RetainageReceivedAllowed { get; set; }

    public int? RetainagePayableAllowed { get; set; }

    public int? CashPlusEarningsGreaterThanBillingsAllowed { get; set; }

    public int? BillingsGreaterThanCostsPlusEarningsAllowed { get; set; }

    public int? PrepaidExpensesAllowed { get; set; }

    public int? CashInventoryAllowed { get; set; }

    public int? OtherCurrentAssetsAllowed { get; set; }

    public int? BankDebtAllowed { get; set; }

    public int? GaexpensesAllowed { get; set; }

    public int? OperatingProfitAllowed { get; set; }

    public int? IncomeAfterTaxesAllowed { get; set; }

    public int? CashAllowed { get; set; }

    public int? NotesAllowed { get; set; }

    public int? QscoreAllowed { get; set; }

    public int? QprofitAllowed { get; set; }

    public int? QnetworthAllowed { get; set; }

    public int? QleverageAllowed { get; set; }

    public int? QworkingCapitalAllowed { get; set; }

    public int? QcratioAllowed { get; set; }

    public string? Scaling { get; set; }

    public int? SingleLoa { get; set; }

    public int? AggregateLoa { get; set; }

    public int? LargestJob { get; set; }

    public int? LargestBacklog { get; set; }

    public int? CashFromOperations { get; set; }

    public int? CashFromInvestments { get; set; }

    public int? CashFromFinancing { get; set; }

    public string? Comments { get; set; }

    public int? NetChangeCash { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual ICollection<BalanceSheet> BalanceSheets { get; set; } = new List<BalanceSheet>();
}
