using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgram
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateTime Effective { get; set; }

    public DateTime Expiration { get; set; }

    public int Single { get; set; }

    public int Aggregate { get; set; }

    public string? Comments { get; set; }

    public Guid StatusId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public string? ApprovedBy { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public bool HomeOfficeApproved { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual ICollection<AccountProgramStatusHistory> AccountProgramStatusHistories { get; set; } = new List<AccountProgramStatusHistory>();
}
