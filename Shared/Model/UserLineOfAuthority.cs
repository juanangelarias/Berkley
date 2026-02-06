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

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Employee ModifiedByNavigation { get; set; } = null!;

    public virtual Employee User { get; set; } = null!;
}
