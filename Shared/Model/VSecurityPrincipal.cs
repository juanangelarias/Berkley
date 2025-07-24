using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class VSecurityPrincipal
{
    public Guid Id { get; set; }

    public string Principal { get; set; } = null!;

    public string? Description { get; set; }
}
