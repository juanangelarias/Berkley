using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyLicense
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid? AgentId { get; set; }

    public string State { get; set; } = null!;

    public string? LicenseNumber { get; set; }

    public bool IsResident { get; set; }

    public Guid InsurerId { get; set; }

    public DateOnly? Expiration { get; set; }

    public string? Comments { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly? Appointment { get; set; }

    public DateOnly? Termination { get; set; }

    public bool? AppointingState { get; set; }

    public virtual LegalEntity? Agent { get; set; }

    public virtual LegalEntity IdNavigation { get; set; } = null!;

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual State StateNavigation { get; set; } = null!;
}
