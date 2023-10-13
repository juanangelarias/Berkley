using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class OpenClaim
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public string ClaimNumber { get; set; } = null!;

    public string AdjusterName { get; set; } = null!;

    public decimal? Reserve { get; set; }

    public virtual Bond BondNumberNavigation { get; set; } = null!;
}
