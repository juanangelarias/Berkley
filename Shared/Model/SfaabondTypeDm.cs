using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class SfaabondTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string SfaabondType { get; set; } = null!;

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();
}
