using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class EmailActionDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Action { get; set; } = null!;

    public virtual ICollection<EmailHistory> EmailHistories { get; set; } = new List<EmailHistory>();
}
