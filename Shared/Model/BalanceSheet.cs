using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BalanceSheet
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly StatementDate { get; set; }

    public string AccountType { get; set; } = null!;

    public int Sequence { get; set; }

    public string? AccountName { get; set; }

    public int? Stated { get; set; }

    public int? Adjustment { get; set; }

    public int? AsAllowed { get; set; }

    public string? Comments { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Ratio Ratio { get; set; } = null!;

    public virtual ICollection<Subaccount> Subaccounts { get; set; } = new List<Subaccount>();
}
