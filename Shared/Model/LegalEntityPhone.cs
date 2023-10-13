using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LegalEntityPhone
{
    public Guid LegalEntityId { get; set; }

    public Guid PhoneNumberId { get; set; }

    public string Type { get; set; } = null!;

    public virtual LegalEntity LegalEntity { get; set; } = null!;

    public virtual PhoneNumber PhoneNumber { get; set; } = null!;

    public virtual PhoneTypeDm TypeNavigation { get; set; } = null!;
}
