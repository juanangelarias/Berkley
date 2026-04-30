using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class TransactionPurposeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string TransactionPurpose { get; set; } = null!;

    public virtual BondTransactionPurpose? BondTransactionPurpose { get; set; }

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();
}
