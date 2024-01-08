using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AppUser
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string ActiveDirectoryAccount { get; set; } = null!;

    public string? Initials { get; set; }

    public string? Title { get; set; }

    public string? Email { get; set; }

    public bool Active { get; set; }
}
