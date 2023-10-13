using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class VoidedBond
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public Guid AgencyId { get; set; }

    public string? Comments { get; set; }

    public Guid VoidedBy { get; set; }

    public virtual Agency Agency { get; set; } = null!;

    public virtual UserProfile VoidedByNavigation { get; set; } = null!;
}
