using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ScalingDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Scaling { get; set; } = null!;

    public virtual ICollection<CashFlowStatement> CashFlowStatements { get; set; } = new List<CashFlowStatement>();

    public virtual ICollection<PersonalFinancialHeader> PersonalFinancialHeaders { get; set; } = new List<PersonalFinancialHeader>();
}
