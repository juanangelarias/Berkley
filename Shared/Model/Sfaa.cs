using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Sfaa
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int Code { get; set; }

    public string? Description { get; set; }

    public string General { get; set; } = null!;

    public string? RateClass { get; set; }

    public string? RiskType { get; set; }

    public bool NmlsclassCode { get; set; }

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercials { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();
}
