using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgentSystemDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string SystemName { get; set; } = null!;

    public virtual ICollection<OnlineBondSystem> OnlineBondSystems { get; set; } = new List<OnlineBondSystem>();
}
