using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class SicratioTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Sicratio> Sicratios { get; set; } = new List<Sicratio>();
}
