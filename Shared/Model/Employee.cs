using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Employee
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string ActiveDirectoryAccount { get; set; } = null!;

    public string Initials { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Title { get; set; }

    public string? Email { get; set; }

    public Guid? PrimaryBranchId { get; set; }

    public bool HomeOfficeApprover { get; set; }

    public bool Active { get; set; }

    public virtual Underwriter? UnderwriterIdNavigation { get; set; }

    public virtual ICollection<Underwriter> UnderwriterReportsToNavigations { get; set; } = new List<Underwriter>();
}
