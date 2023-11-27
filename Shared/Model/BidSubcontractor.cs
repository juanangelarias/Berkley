using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BidSubcontractor
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BidNumber { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Trade { get; set; }

    public int? Amount { get; set; }

    public bool Bonded { get; set; }

    public virtual BondRequest BidNumberNavigation { get; set; } = null!;
}
