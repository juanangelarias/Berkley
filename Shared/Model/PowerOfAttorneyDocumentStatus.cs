using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PowerOfAttorneyDocumentStatus
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid Poaid { get; set; }

    public DateOnly? Requested { get; set; }

    public DateOnly? Received { get; set; }

    public string? Name { get; set; }

    public virtual PowerOfAttorneyDocumentNameDm? NameNavigation { get; set; }

    public virtual PowerOfAttorney Poa { get; set; } = null!;
}
