using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PhoneNumber
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string CountryCode { get; set; } = null!;

    public string MainNumber { get; set; } = null!;

    public string? Extension { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Agent> AgentDefaultCellNumberNavigations { get; set; } = new List<Agent>();

    public virtual ICollection<Agent> AgentDefaultPhoneNumberNavigations { get; set; } = new List<Agent>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual LegalEntityPhone? LegalEntityPhone { get; set; }
}
