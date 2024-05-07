using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class PowerOfAttorneyDocumentStatus
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid Poaid { get; set; }

    public DateOnly? Requested { get; set; }

    public DateOnly? Received { get; set; }

    public Guid DocumentTypeId { get; set; }

    public string? Comments { get; set; }

    public virtual PowerOfAttorneyDocumentNameDm DocumentType { get; set; } = null!;

    public virtual PowerOfAttorney Poa { get; set; } = null!;
}
