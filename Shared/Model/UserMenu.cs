using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UserMenu
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Username { get; set; } = null!;

    public string MenuName { get; set; } = null!;

    public int MenuOrder { get; set; }

    public string Text { get; set; } = null!;

    public string? Icon { get; set; }

    public string UrlPattern { get; set; } = null!;

    public bool MinimalDisplay { get; set; }
}
