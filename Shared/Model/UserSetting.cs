using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UserSetting
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Username { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}
