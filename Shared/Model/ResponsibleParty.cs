using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ResponsibleParty
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Type { get; set; } = null!;

    public bool PrintStatusLetter { get; set; }

    public virtual ResponsiblePartyTypeDm TypeNavigation { get; set; } = null!;
}
