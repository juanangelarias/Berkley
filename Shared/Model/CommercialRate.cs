using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CommercialRate
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string RateGroup { get; set; } = null!;

    public string CommercialBondType { get; set; } = null!;

    public string RiskType { get; set; } = null!;

    public int MinumumAmount { get; set; }

    public int MaximumAmount { get; set; }

    public double? AmountPerUnit { get; set; }

    public int? UnitSize { get; set; }

    public int? AnnualMinimum { get; set; }

    public virtual CommercialBondTypeDm CommercialBondTypeNavigation { get; set; } = null!;

    public virtual RateGroupDm RateGroupNavigation { get; set; } = null!;

    public virtual RiskTypeDm RiskTypeNavigation { get; set; } = null!;
}
