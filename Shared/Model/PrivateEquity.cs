using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PrivateEquity
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string FirmName { get; set; } = null!;

    public string Year { get; set; } = null!;

    public Guid EnteredBy { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Employee EnteredByNavigation { get; set; } = null!;
}
