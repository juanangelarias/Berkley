using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class RateStructureDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string RateStructure { get; set; } = null!;
}
