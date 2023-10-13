using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountReference
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string? Type { get; set; }

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual ReferenceTypeDm? TypeNavigation { get; set; }
}
