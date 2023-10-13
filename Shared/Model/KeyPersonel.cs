using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class KeyPersonel
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string? EncryptYearOfBirth { get; set; }

    public string? Position { get; set; }

    public string? Responsibility { get; set; }

    public string? YearJoinedCompany { get; set; }

    public string? YearEnteredField { get; set; }

    public string? SpouseName { get; set; }

    public bool PersonalIndemnification { get; set; }

    public string? Comments { get; set; }

    public string? Profession { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual ResponsibilityDm? ResponsibilityNavigation { get; set; }
}
