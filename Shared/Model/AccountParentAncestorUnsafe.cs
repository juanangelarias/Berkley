using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountParentAncestorUnsafe
{
    public Guid? Id { get; set; }

    public string? AccountNum { get; set; }

    public Guid? ParentId { get; set; }

    public string? ParentAcctNumber { get; set; }

    public Guid? AncestorId { get; set; }

    public string? AncestorAccountNum { get; set; }
}
