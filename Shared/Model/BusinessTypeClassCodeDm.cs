using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BusinessTypeClassCodeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BusinessType { get; set; } = null!;

    public int ClassCode { get; set; }

    public bool IsContract { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
