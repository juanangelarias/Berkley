using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountStatusLog
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string AccountStatus { get; set; } = null!;

    public Guid ModifiedBy { get; set; }

    public string? Comments { get; set; }

    public DateTime Effective { get; set; }

    public virtual AccountStatusDm AccountStatusNavigation { get; set; } = null!;

    public virtual UserProfile ModifiedByNavigation { get; set; } = null!;
}
