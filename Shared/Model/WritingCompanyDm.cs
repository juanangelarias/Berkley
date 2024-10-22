using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class WritingCompanyDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Insurer> Insurers { get; set; } = new List<Insurer>();
}
