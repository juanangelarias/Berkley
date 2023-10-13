using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CoPrincipal
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public string AccountNum { get; set; } = null!;

    public string? Profession { get; set; }

    public int? YearsInProfession { get; set; }

    public double? LiabilityAmount { get; set; }

    public string? Comments { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Bond BondNumberNavigation { get; set; } = null!;
}
