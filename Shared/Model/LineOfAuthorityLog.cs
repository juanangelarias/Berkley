using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LineOfAuthorityLog
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public int SequenceNumber { get; set; }

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public int Loasingle { get; set; }

    public int Loaaggregate { get; set; }

    public string? Comments { get; set; }

    public string? Status { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? Approved { get; set; }

    public string? Division { get; set; }

    public string? BondType { get; set; }

    public string? Conditions { get; set; }

    public bool HomeOfficeApproved { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual UserProfile? ApprovedByNavigation { get; set; }

    public virtual UserProfile? CreatedByNavigation { get; set; }

    public virtual DivisionDm? DivisionNavigation { get; set; }

    public virtual LineOfAuthorityStatusDm? StatusNavigation { get; set; }
}
