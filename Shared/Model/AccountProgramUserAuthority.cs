using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgramUserAuthority
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid UserId { get; set; }

    public Guid AccountClassId { get; set; }

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public int Single { get; set; }

    public int Aggregate { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid ModifiedBy { get; set; }

    public virtual UserProfile CreatedByNavigation { get; set; } = null!;

    public virtual UserProfile ModifiedByNavigation { get; set; } = null!;

    public virtual UserProfile User { get; set; } = null!;
}
