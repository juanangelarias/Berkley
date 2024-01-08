using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Collateral
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string? Type { get; set; }

    public int? Amount { get; set; }

    public DateOnly? Expiration { get; set; }

    public bool AutoRenew { get; set; }

    public string? Description { get; set; }

    public string? BondNumber { get; set; }

    public bool? BondSpecific { get; set; }

    public DateTime? Released { get; set; }

    public virtual Bond? BondNumberNavigation { get; set; }
}
