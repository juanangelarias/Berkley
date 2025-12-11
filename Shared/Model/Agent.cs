using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Agent
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string? NationalProducerNumber { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<AgencyLicense> AgencyLicenses { get; set; } = new List<AgencyLicense>();

    public virtual ICollection<AgencyLineOfAuthorityAgent> AgencyLineOfAuthorityAgents { get; set; } = new List<AgencyLineOfAuthorityAgent>();

    public virtual ICollection<AgentsInAgency> AgentsInAgencies { get; set; } = new List<AgentsInAgency>();

    public virtual LegalEntity IdNavigation { get; set; } = null!;
}
