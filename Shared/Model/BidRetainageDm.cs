using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BidRetainageDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Retainage { get; set; } = null!;

    public virtual ICollection<BidRequest> BidRequests { get; set; } = new List<BidRequest>();
}
