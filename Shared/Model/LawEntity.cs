using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LawEntity
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string? MartindaleHubbellRating { get; set; }

    public string? Comment { get; set; }

    public virtual ICollection<Account> AccountAttorneys { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountLawFirms { get; set; } = new List<Account>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual LegalEntity IdNavigation { get; set; } = null!;
}
