using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Sic
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string General { get; set; } = null!;

    public string? RiskLevel { get; set; }

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual ICollection<Sicratio> Sicratios { get; set; } = new List<Sicratio>();
}
