using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CommercialFinancial
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string Period { get; set; } = null!;

    public long? Cash { get; set; }

    public long? TotalAssets { get; set; }

    public long? CurrentLiabilities { get; set; }

    public long? LongTermLiabilities { get; set; }

    public long? LongTermDebt { get; set; }

    public long? Revenue { get; set; }

    public long? CostOfGoodsSold { get; set; }

    public long? Interest { get; set; }

    public long? Taxes { get; set; }

    public long? NetIncome { get; set; }

    public long? Depreciation { get; set; }

    public long? Amortization { get; set; }

    public long? CapitalExpenditures { get; set; }

    public long? NetCashFromOperations { get; set; }

    public long? GrossProfit { get; set; }

    public long? TotalLiabilities { get; set; }

    public long? TotalDebt { get; set; }

    public string Scaling { get; set; } = null!;

    public bool Complete { get; set; }

    public long? ShortTermDebt { get; set; }

    public virtual ScalingDm ScalingNavigation { get; set; } = null!;
}
