using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LegalEntityEmail
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid LegalEntityId { get; set; }

    public string Type { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public virtual LegalEntity LegalEntity { get; set; } = null!;

    public virtual EmailTypeDm TypeNavigation { get; set; } = null!;
}
