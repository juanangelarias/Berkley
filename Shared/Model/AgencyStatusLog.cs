using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyStatusLog
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AgencyNumber { get; set; } = null!;

    public DateTime Effective { get; set; }

    public string? OldStatus { get; set; }

    public string? NewStatus { get; set; }

    public Guid? ChangedBy { get; set; }

    public string? Comments { get; set; }

    public virtual Agency AgencyNumberNavigation { get; set; } = null!;

    public virtual UserProfile? ChangedByNavigation { get; set; }
}
