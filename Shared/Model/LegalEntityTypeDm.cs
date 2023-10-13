using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class LegalEntityTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string EntityType { get; set; } = null!;

    public virtual ICollection<LegalEntity> LegalEntities { get; set; } = new List<LegalEntity>();
}
