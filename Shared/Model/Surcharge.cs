using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Surcharge
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string State { get; set; } = null!;

    public double Amount { get; set; }

    public DateOnly Effective { get; set; }

    public DateOnly Expiration { get; set; }

    public bool Exclude { get; set; }

    public string Type { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public virtual UserProfile CreatedByNavigation { get; set; } = null!;

    public virtual State StateNavigation { get; set; } = null!;

    public virtual SurchargeTypeDm TypeNavigation { get; set; } = null!;
}
