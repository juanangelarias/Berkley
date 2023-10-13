using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyStatusDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Agency> Agencies { get; set; } = new List<Agency>();
}
