using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyCommission
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AgencyId { get; set; }

    public string BondType { get; set; } = null!;

    public int Minimum { get; set; }

    public int? Maximum { get; set; }

    public double Rate { get; set; }

    public DateTime Effective { get; set; }

    public DateTime? Expiration { get; set; }

    public int ExpireIncluded { get; set; }
}
