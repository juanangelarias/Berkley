using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Currency
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Ord { get; set; }

    public bool Active { get; set; }
}
