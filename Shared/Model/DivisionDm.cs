using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class DivisionDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string DivisionCode { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string LoanotificationGroup { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<ImagingCategoryTabDivision> ImagingCategoryTabDivisions { get; set; } = new List<ImagingCategoryTabDivision>();

    public virtual ICollection<LineOfAuthorityLog> LineOfAuthorityLogs { get; set; } = new List<LineOfAuthorityLog>();

    public virtual ICollection<ProfitCenter> ProfitCenters { get; set; } = new List<ProfitCenter>();
}
