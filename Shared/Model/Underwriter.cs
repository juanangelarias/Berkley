using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Underwriter
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string? NotificationSendTo { get; set; }

    public bool DropDownList { get; set; }

    public Guid? ReportsTo { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<BondHold> BondHolds { get; set; } = new List<BondHold>();

    public virtual ICollection<BondRequestCommercial> BondRequestCommercials { get; set; } = new List<BondRequestCommercial>();

    public virtual ICollection<BondRequest> BondRequests { get; set; } = new List<BondRequest>();

    public virtual ICollection<BondTransaction> BondTransactions { get; set; } = new List<BondTransaction>();

    public virtual ICollection<Bond> Bonds { get; set; } = new List<Bond>();

    public virtual Employee IdNavigation { get; set; } = null!;

    public virtual ICollection<ProfitCenter> ProfitCenters { get; set; } = new List<ProfitCenter>();

    public virtual Employee? ReportsToNavigation { get; set; }
}
