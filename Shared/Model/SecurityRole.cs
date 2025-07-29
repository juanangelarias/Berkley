using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class SecurityRole
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Role { get; set; } = null!;

    public int Ord { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Security> Securities { get; set; } = new List<Security>();
}
