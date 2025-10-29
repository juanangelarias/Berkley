using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class OnlineBondSystem
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid LegalEntityId { get; set; }

    public string SystemName { get; set; } = null!;

    public Guid InsurerId { get; set; }

    public int? WritingLimit { get; set; }

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual LegalEntity LegalEntity { get; set; } = null!;

    public virtual AgentSystemDm SystemNameNavigation { get; set; } = null!;
}
