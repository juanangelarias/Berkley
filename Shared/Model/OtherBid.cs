using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class OtherBid
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BidNumber { get; set; } = null!;

    public string Bidder { get; set; } = null!;

    public int Amount { get; set; }

    public virtual BondRequest BidNumberNavigation { get; set; } = null!;
}
