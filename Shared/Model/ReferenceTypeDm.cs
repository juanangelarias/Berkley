using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ReferenceTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<AccountReference> AccountReferences { get; set; } = new List<AccountReference>();
}
