using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ObligeeContact
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Title { get; set; } = null!;

    public virtual LegalEntity IdNavigation { get; set; } = null!;
}
