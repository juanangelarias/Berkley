using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AuthorityUsedDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AuthorityUsed { get; set; } = null!;
}
