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

    public string? ReferenceNumber { get; set; }

    public DateOnly? FirstIssued { get; set; }

    public DateOnly? CurrentIssued { get; set; }

    public string? Comments { get; set; }

    public Guid? ImagingId { get; set; }

    public string Status { get; set; } = null!;

    public virtual Agency Agency { get; set; } = null!;

    public virtual Insurer Insurer { get; set; } = null!;

    public virtual ICollection<PowerOfAttorneyDocumentStatus> PowerOfAttorneyDocumentStatuses { get; set; } = new List<PowerOfAttorneyDocumentStatus>();

    public virtual PowerOfAttorneyStatusDm StatusNavigation { get; set; } = null!;
}
