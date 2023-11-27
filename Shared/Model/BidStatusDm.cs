using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BidStatusDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Status { get; set; } = null!;

    public int Order { get; set; }

    public virtual ICollection<BondRequestCommercial> BondRequestCommercials { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequest> BondRequests { get; set; } = new List<BondRequest>();
}
