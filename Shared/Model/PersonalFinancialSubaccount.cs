using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PersonalFinancialSubaccount
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid ParentAccountId { get; set; }

    public int Sequence { get; set; }

    public int? Stated { get; set; }

    public int? Adjustment { get; set; }

    public int? AsAllowed { get; set; }

    public string? Comments { get; set; }

    public virtual PersonalFinancialStatement ParentAccount { get; set; } = null!;
}
