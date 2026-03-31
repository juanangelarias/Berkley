using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondBlockAllowedAgency
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid BondBlockId { get; set; }

    public string AgencyNumber { get; set; } = null!;

    public virtual Agency AgencyNumberNavigation { get; set; } = null!;

    public virtual BondBlock BondBlock { get; set; } = null!;
}
