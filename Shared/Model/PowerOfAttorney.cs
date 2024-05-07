using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PowerOfAttorney
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid InsurerId { get; set; }

    public Guid AgencyId { get; set; }

    public int? Limit { get; set; }

    public string? Serial { get; set; }

    public DateOnly? FirstIssued { get; set; }

    public DateOnly? CurrentIssued { get; set; }

    public string? Comments { get; set; }

    public Guid Status { get; set; }

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual ICollection<OnlineBondSystem> OnlineBondSystems { get; set; } = new List<OnlineBondSystem>();

    public virtual ICollection<PowerOfAttorneyDocumentStatus> PowerOfAttorneyDocumentStatuses { get; set; } = new List<PowerOfAttorneyDocumentStatus>();

    public virtual PowerOfAttorneyStatusDm StatusNavigation { get; set; } = null!;
}
