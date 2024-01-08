using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UserLineOfAuthority
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid UserId { get; set; }

    public int Single { get; set; }

    public int Aggregate { get; set; }

    public DateOnly Expiration { get; set; }

    public string DivisionCode { get; set; } = null!;

    public string BondType { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public virtual UserProfile CreatedByNavigation { get; set; } = null!;

    public virtual UserProfile ModifiedByNavigation { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
