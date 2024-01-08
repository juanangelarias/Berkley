using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BondStatusLetter
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public DateOnly Sent { get; set; }

    public DateOnly? ReceivedBack { get; set; }

    public double? PercentComplete { get; set; }

    public virtual Bond BondNumberNavigation { get; set; } = null!;
}
