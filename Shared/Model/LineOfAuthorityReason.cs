using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LineOfAuthorityReason
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public string Type { get; set; } = null!;

    public string? Recommendation { get; set; }

    public string? BusinessOverview { get; set; }

    public string? BondRisk { get; set; }

    public string? FinancialAnalysis { get; set; }

    public string? DebtHighlights { get; set; }

    public string? FollowUpConditions { get; set; }

    public string? KeyChanges { get; set; }

    public string? Outlook { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<LineOfAuthorityLog> LineOfAuthorityLogs { get; set; } = new List<LineOfAuthorityLog>();
}
