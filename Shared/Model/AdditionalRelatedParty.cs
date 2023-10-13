using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AdditionalRelatedParty
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public bool Indemnity { get; set; }

    public bool FinancialStatements { get; set; }

    public bool Guarantor { get; set; }

    public string? Comments { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual LegalEntity IdNavigation { get; set; } = null!;
}
