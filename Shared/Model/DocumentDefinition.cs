using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class DocumentDefinition
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActiveGiaform { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DocumentDefinitionRule> DocumentDefinitionRules { get; set; } = new List<DocumentDefinitionRule>();
}
