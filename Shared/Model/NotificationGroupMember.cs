using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class NotificationGroupMember
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string GroupName { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual NotificationGroup GroupNameNavigation { get; set; } = null!;
}
