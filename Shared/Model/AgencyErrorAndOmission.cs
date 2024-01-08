using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyErrorAndOmission
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AgencyId { get; set; }

    public string Carrier { get; set; } = null!;

    public DateOnly Expiration { get; set; }

    public int CoverageLimit { get; set; }

    public virtual Agency Agency { get; set; } = null!;
}
