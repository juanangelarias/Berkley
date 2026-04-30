using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class FinancialDetail
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid StatementId { get; set; }

    public int Sequence { get; set; }

    public string AccountType { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public long? Stated { get; set; }

    public long? Allowed { get; set; }

    public string? Comments { get; set; }

    public Guid? ParentId { get; set; }

    public Guid PkparentId { get; set; }

    public virtual ICollection<FinancialDetail> InverseParent { get; set; } = new List<FinancialDetail>();

    public virtual FinancialDetail? Parent { get; set; }

    public virtual FinancialStatement Statement { get; set; } = null!;
}
