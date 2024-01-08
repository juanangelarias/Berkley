using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondType { get; set; } = null!;

    public string BondClass { get; set; } = null!;

    public string? Description { get; set; }

    public string? Class { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();
}
