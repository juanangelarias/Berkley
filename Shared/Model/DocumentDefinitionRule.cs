using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class DocumentDefinitionRule
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid DefinitionId { get; set; }

    public Guid RuleId { get; set; }

    public virtual DocumentDefinition Definition { get; set; } = null!;

    public virtual DocumentRule Rule { get; set; } = null!;
}
