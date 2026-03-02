using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyLineOfAuthorityLog
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AgencyNumber { get; set; } = null!;

    public string AccountNum { get; set; } = null!;

    public int SequenceNumber { get; set; }

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public int Loasingle { get; set; }

    public int Loaaggregate { get; set; }

    public string? Comments { get; set; }

    public string? Division { get; set; }

    public string? BondType { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? Conditions { get; set; }

    public virtual ICollection<AgencyLineOfAuthorityAgent> AgencyLineOfAuthorityAgents { get; set; } = new List<AgencyLineOfAuthorityAgent>();

    public virtual Employee? CreatedByNavigation { get; set; }
}
