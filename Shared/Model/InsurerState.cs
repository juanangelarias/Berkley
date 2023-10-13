using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class InsurerState
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid InsurerId { get; set; }

    public string State { get; set; } = null!;

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual State StateNavigation { get; set; } = null!;
}
