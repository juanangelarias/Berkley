using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountClassDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountClass { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? DivisionGeneralLedgerCode { get; set; }

    public bool Active { get; set; }

    public string? LineOfAuthorityNotificationGroup { get; set; }

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual ICollection<DefaultGeneralLedgerAccount> DefaultGeneralLedgerAccounts { get; set; } = new List<DefaultGeneralLedgerAccount>();
}
