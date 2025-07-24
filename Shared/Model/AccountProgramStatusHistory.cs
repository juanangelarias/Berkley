using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountProgramStatusHistory
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid? OldStatus { get; set; }

    public Guid NewStatus { get; set; }

    public int? OldSingle { get; set; }

    public int NewSingle { get; set; }

    public int? OldAggregate { get; set; }

    public int NewAggregate { get; set; }

    public DateTime StatusDate { get; set; }

    public Guid StatusChangeBy { get; set; }

    public Guid AccountProgramId { get; set; }

    public string AccountNum { get; set; } = null!;

    public virtual AccountProgram AccountProgram { get; set; } = null!;

    public virtual AccountProgramStatusDm NewStatusNavigation { get; set; } = null!;

    public virtual AccountProgramStatusDm? OldStatusNavigation { get; set; }
}
