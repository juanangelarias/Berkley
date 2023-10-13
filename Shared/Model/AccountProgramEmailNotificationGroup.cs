using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgramEmailNotificationGroup
{
    public int Id { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public string UserName { get; set; } = null!;

    public string? SendTo { get; set; }
}
