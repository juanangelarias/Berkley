using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Naicscode
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Code { get; set; } = null!;

    public string Sector { get; set; } = null!;

    public string? Description { get; set; }
}
