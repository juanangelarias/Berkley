using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class IndustryCodeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Code { get; set; } = null!;

    public string Industry { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
