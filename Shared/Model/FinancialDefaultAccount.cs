using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialDefaultAccount
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountClass { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public int Sequence { get; set; }

    public string AccountName { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public Guid PkparentId { get; set; }

    public virtual ICollection<FinancialDefaultAccount> InverseParent { get; set; } = new List<FinancialDefaultAccount>();

    public virtual FinancialDefaultAccount? Parent { get; set; }
}
