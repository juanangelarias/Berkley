using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UserLayoutWidget
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Username { get; set; } = null!;

    public int ColumnOrder { get; set; }

    public int WidgetOrder { get; set; }

    public Guid WidgetId { get; set; }

    public virtual UserLayoutColumn UserLayoutColumn { get; set; } = null!;
}
