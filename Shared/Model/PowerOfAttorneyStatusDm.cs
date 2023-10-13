using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PowerOfAttorneyStatusDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PowerOfAttorney> PowerOfAttorneys { get; set; } = new List<PowerOfAttorney>();
}
