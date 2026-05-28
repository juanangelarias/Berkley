using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialAccountTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string FinancialAccountType { get; set; } = null!;

    public string? Title { get; set; }

    public string UsedIn { get; set; } = null!;
}
