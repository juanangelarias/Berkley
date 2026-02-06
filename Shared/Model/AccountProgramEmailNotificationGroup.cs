using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgramEmailNotificationGroup
{
    public Guid Id { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public Guid UserId { get; set; }

    public string? SendTo { get; set; }

    public virtual Employee User { get; set; } = null!;
}
