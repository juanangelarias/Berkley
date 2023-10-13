using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class InventoryDocumentDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string DocumentType { get; set; } = null!;

    public virtual ICollection<AgencyInventory> AgencyInventories { get; set; } = new List<AgencyInventory>();
}
