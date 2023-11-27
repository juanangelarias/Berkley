using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class RateGroupDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string RateGroup { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public virtual ICollection<CommercialRate> CommercialRates { get; set; } = new List<CommercialRate>();

    public virtual ICollection<ContractRate> ContractRates { get; set; } = new List<ContractRate>();
}
