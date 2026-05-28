using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PersonalFinancialFor
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public string? Comment { get; set; }

    public virtual FinancialStatement IdNavigation { get; set; } = null!;
}
