using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AdditionalObligee
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid ObligeeNum { get; set; }

    public bool PrintStatusLetter { get; set; }

    public string? Interest { get; set; }

    public string BondNumber { get; set; } = null!;

    public virtual Bond BondNumberNavigation { get; set; } = null!;
}
