using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgentsInAgency
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AgentId { get; set; }

    public Guid AgencyId { get; set; }

    public bool AttorneyInFact { get; set; }

    public bool PortalUser { get; set; }

    public bool Active { get; set; }

    public string? Comments { get; set; }

    public virtual Agency Agency { get; set; } = null!;

    public virtual Agent Agent { get; set; } = null!;
}
