using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondTransactionPurpose
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public Guid TransactionGroup { get; set; }

    public string TransactionPurpose { get; set; } = null!;

    public int BondMod { get; set; }

    public int GroupNumber { get; set; }

    public string OldValue { get; set; } = null!;

    public string NewValue { get; set; } = null!;

    public virtual Bond BondNumberNavigation { get; set; } = null!;

    public virtual BondTransaction BondTransaction { get; set; } = null!;
}
