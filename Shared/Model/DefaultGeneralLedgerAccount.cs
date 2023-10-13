using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class DefaultGeneralLedgerAccount
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountName { get; set; } = null!;

    public string AccountClass { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public bool FundedDebt { get; set; }

    public virtual AccountClassDm AccountClassNavigation { get; set; } = null!;
}
