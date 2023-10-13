using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class OnlineBondSystem
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AgencyId { get; set; }

    public string SystemName { get; set; } = null!;

    public Guid InsurerId { get; set; }

    public Guid PowerOfAttorneyId { get; set; }

    public virtual Agency Agency { get; set; } = null!;

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual PowerOfAttorney PowerOfAttorney { get; set; } = null!;

    public virtual AgentSystemDm SystemNameNavigation { get; set; } = null!;
}
