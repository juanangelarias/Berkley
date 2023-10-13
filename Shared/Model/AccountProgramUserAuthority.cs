using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgramUserAuthority
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string User { get; set; } = null!;

    public Guid AccountClassId { get; set; }

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public int Single { get; set; }

    public int Aggregate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string ModifiedBy { get; set; } = null!;
}
