using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ObligeeTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Obligee> Obligees { get; set; } = new List<Obligee>();
}
