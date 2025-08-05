using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BusinessTypeRiskCodeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BusinessType { get; set; } = null!;

    public string? SubType { get; set; }

    public int RiskCode { get; set; }

    public bool IsContract { get; set; }

    public string OldName { get; set; } = null!;

    public string NewName { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
