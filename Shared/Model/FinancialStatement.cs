using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialStatement
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly StatementDate { get; set; }

    public int Version { get; set; }

    public string FinancialType { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Basis { get; set; } = null!;

    public string Quality { get; set; } = null!;

    public string Scaling { get; set; } = null!;

    public string? TaxBasis { get; set; }

    public long? CashFromOperations { get; set; }

    public long? CashFromInvestments { get; set; }

    public long? CashFromFinancing { get; set; }

    public long? NetChangeCash { get; set; }

    public string? Comments { get; set; }

    public Guid? ImagingId { get; set; }

    public bool BalanceSheetCompleted { get; set; }

    public bool ProfitLossStatementCompleted { get; set; }

    public bool Completed { get; set; }

    public virtual StatementBasisDm BasisNavigation { get; set; } = null!;

    public virtual ICollection<FinancialAggregate> FinancialAggregates { get; set; } = new List<FinancialAggregate>();

    public virtual ICollection<FinancialDetail> FinancialDetails { get; set; } = new List<FinancialDetail>();

    public virtual ICollection<FinancialRatio> FinancialRatios { get; set; } = new List<FinancialRatio>();

    public virtual FinancialTypeDm FinancialTypeNavigation { get; set; } = null!;

    public virtual PersonalFinancialFor? PersonalFinancialFor { get; set; }

    public virtual StatementQualityDm QualityNavigation { get; set; } = null!;

    public virtual ScalingDm ScalingNavigation { get; set; } = null!;

    public virtual StatementTypeDm TypeNavigation { get; set; } = null!;
}
