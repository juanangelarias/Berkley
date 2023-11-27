using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyCompetition
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public Guid AgencyId { get; set; }

    public int AnnualBusiness { get; set; }

    public int? AnnualCommission { get; set; }

    public Guid EnteredBy { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Agency Agency { get; set; } = null!;

    public virtual UserProfile EnteredByNavigation { get; set; } = null!;
}
