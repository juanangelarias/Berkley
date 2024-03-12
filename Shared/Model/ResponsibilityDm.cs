using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ResponsibilityDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Responsibility { get; set; } = null!;

    public virtual ICollection<KeyPersonnel> KeyPersonnel { get; set; } = new List<KeyPersonnel>();
}
