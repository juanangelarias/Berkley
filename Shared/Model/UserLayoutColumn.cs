using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UserLayoutColumn
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Username { get; set; } = null!;

    public int ColumnOrder { get; set; }

    public bool PrimaryScreen { get; set; }

    public virtual ICollection<UserLayoutWidget> UserLayoutWidgets { get; set; } = new List<UserLayoutWidget>();
}
