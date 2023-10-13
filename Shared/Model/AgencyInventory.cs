using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AgencyInventory
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AgencyId { get; set; }

    public DateTime? Sent { get; set; }

    public int? Quantity { get; set; }

    public string DocumentType { get; set; } = null!;

    public Guid? AddresseeId { get; set; }

    public Guid Approver { get; set; }

    public virtual LegalEntity? Addressee { get; set; }

    public virtual Agency Agency { get; set; } = null!;

    public virtual UserProfile ApproverNavigation { get; set; } = null!;

    public virtual InventoryDocumentDm DocumentTypeNavigation { get; set; } = null!;
}
