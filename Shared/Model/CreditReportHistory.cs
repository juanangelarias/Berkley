using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CreditReportHistory
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string CreditReport { get; set; } = null!;

    public DateTime Pulled { get; set; }

    public string? Rating { get; set; }

    public string? Definition { get; set; }

    public string? Remarks { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;
}
