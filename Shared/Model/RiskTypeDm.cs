using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class RiskTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string RiskType { get; set; } = null!;

    public virtual ICollection<CommercialRate> CommercialRates { get; set; } = new List<CommercialRate>();
}
