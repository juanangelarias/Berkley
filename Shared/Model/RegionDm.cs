using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class RegionDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Region { get; set; } = null!;

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
