using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class WatchStatusDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string WatchStatus { get; set; } = null!;

    public virtual ICollection<AccountWatch> AccountWatches { get; set; } = new List<AccountWatch>();
}
