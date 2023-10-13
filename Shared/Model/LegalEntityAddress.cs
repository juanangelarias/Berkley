using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LegalEntityAddress
{
    public Guid LegalEntityId { get; set; }

    public Guid AddressId { get; set; }

    public string Type { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual LegalEntity LegalEntity { get; set; } = null!;

    public virtual AddressTypeDm TypeNavigation { get; set; } = null!;
}
