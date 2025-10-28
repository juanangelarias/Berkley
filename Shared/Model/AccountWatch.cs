using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountWatch
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public DateTime WatchDate { get; set; }

    public string WatchStatus { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string ActionPlan { get; set; } = null!;

    public string AccountNum { get; set; } = null!;

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual WatchStatusDm WatchStatusNavigation { get; set; } = null!;
}
