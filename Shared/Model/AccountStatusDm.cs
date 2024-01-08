using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountStatusDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountStatus { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<AccountStatusLog> AccountStatusLogs { get; set; } = new List<AccountStatusLog>();
}
