using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgramStatusDm
{
    public Guid Id { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Modified { get; set; }

    public string Description { get; set; } = null!;

    public int? OldId { get; set; }
}
