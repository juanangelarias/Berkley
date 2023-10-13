using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AddressTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Type { get; set; } = null!;

    public int Order { get; set; }

    public virtual ICollection<LegalEntityAddress> LegalEntityAddresses { get; set; } = new List<LegalEntityAddress>();
}
