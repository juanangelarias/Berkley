using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Obligee
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string ObligeeNum { get; set; } = null!;

    public string? Type { get; set; }

    public Guid? EditedBy { get; set; }

    public bool PrintStatusLetter { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<BondRequest> BondRequests { get; set; } = new List<BondRequest>();

    public virtual UserProfile? EditedByNavigation { get; set; }

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual ObligeeTypeDm? TypeNavigation { get; set; }
}
