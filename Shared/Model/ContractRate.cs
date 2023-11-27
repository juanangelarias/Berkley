using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ContractRate
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string RateGroup { get; set; } = null!;

    public string Class { get; set; } = null!;

    public string RateType { get; set; } = null!;

    public int MinumumAmount { get; set; }

    public int MaximumAmount { get; set; }

    public double? PremiumRate { get; set; }

    public virtual RateGroupDm RateGroupNavigation { get; set; } = null!;
}
