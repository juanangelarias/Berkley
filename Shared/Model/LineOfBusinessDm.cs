using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LineOfBusinessDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string LineOfBusiness { get; set; } = null!;

    public virtual ICollection<ProfitCenter> ProfitCenters { get; set; } = new List<ProfitCenter>();
}
