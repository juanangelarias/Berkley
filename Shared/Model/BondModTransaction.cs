using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondModTransaction
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public int BondMod { get; set; }

    public DateOnly Effective { get; set; }

    public DateOnly Expiration { get; set; }

    public int ModPremium { get; set; }

    public string Status { get; set; } = null!;
}
