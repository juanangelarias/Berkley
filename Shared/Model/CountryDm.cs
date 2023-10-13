using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CountryDm
{
    public string Code { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Insurer> Insurers { get; set; } = new List<Insurer>();

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
