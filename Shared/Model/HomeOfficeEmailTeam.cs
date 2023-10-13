using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class HomeOfficeEmailTeam
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Team { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Email { get; set; } = null!;
}
