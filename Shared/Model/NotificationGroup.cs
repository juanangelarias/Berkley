using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class NotificationGroup
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<NotificationGroupMember> NotificationGroupMembers { get; set; } = new List<NotificationGroupMember>();

    public virtual ICollection<Underwriter> Underwriters { get; set; } = new List<Underwriter>();
}
