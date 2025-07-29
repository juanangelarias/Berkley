using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CreditReportDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string CreditReport { get; set; } = null!;
}
