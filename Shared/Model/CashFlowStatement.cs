using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CashFlowStatement
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly StatementDate { get; set; }

    public string Type { get; set; } = null!;

    public string Basis { get; set; } = null!;

    public string Quality { get; set; } = null!;

    public string Scaling { get; set; } = null!;

    public string? TaxBasis { get; set; }

    public int? NetIncome { get; set; }

    public int? DepreciationAmoritization { get; set; }

    public int? AccountsReceivable { get; set; }

    public int? AccountsReceivableRetention { get; set; }

    public int? CostEarningsExceedingBillings { get; set; }

    public int? AccountsPayable { get; set; }

    public int? AccountsPayableRetention { get; set; }

    public int? BillingsExceedingCostEarnings { get; set; }

    public int? AllOtherCashFlow { get; set; }

    public int? TotalCashFlowsOperations { get; set; }

    public int? NetFixedAssetsAcquired { get; set; }

    public int? AllOtherInvestments { get; set; }

    public int? TotalCashInvestments { get; set; }

    public int? Distributions { get; set; }

    public int? LineOfCredit { get; set; }

    public int? TermDebt { get; set; }

    public int? StockholderNotes { get; set; }

    public int? AllOtherFinancing { get; set; }

    public int? TotalCashFinancing { get; set; }

    public int? NetChangeInCash { get; set; }

    public int? NetFixedAssetsAcquiredDebt { get; set; }

    public bool Completed { get; set; }

    public virtual StatementBasisDm BasisNavigation { get; set; } = null!;

    public virtual StatementQualityDm QualityNavigation { get; set; } = null!;

    public virtual ScalingDm ScalingNavigation { get; set; } = null!;

    public virtual StatementTypeDm TypeNavigation { get; set; } = null!;
}
