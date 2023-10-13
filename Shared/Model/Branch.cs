using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Branch
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BranchKey { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid BranchOfficeAddressId { get; set; }

    public Guid PhoneId { get; set; }

    public bool HomeOffice { get; set; }

    public string EmailReceiver { get; set; } = null!;

    public string? Bccreceiver { get; set; }

    public string? GeneralLedgerCode { get; set; }

    public bool Active { get; set; }

    public string Region { get; set; } = null!;

    public int? HeadCount { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual Address BranchOfficeAddress { get; set; } = null!;

    public virtual PhoneNumber Phone { get; set; } = null!;

    public virtual RegionDm RegionNavigation { get; set; } = null!;
}
