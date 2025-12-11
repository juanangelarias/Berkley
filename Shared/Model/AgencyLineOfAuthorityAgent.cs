using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyLineOfAuthorityAgent
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AgencyLineOfAuthorityId { get; set; }

    public Guid AgentId { get; set; }

    public virtual AgencyLineOfAuthorityLog AgencyLineOfAuthority { get; set; } = null!;

    public virtual Agent Agent { get; set; } = null!;
}
