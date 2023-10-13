using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class EmailHistory
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string From { get; set; } = null!;

    public string FromName { get; set; } = null!;

    public string To { get; set; } = null!;

    public string ToName { get; set; } = null!;

    public string? Cc { get; set; }

    public string AccountNum { get; set; } = null!;

    public string BondNumber { get; set; } = null!;

    public DateTime Sent { get; set; }

    public string Action { get; set; } = null!;

    public bool IsContract { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual EmailActionDm ActionNavigation { get; set; } = null!;

    public virtual Bond BondNumberNavigation { get; set; } = null!;
}
