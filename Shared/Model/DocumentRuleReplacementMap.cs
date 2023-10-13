using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class DocumentRuleReplacementMap
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid RuleId { get; set; }

    public string Token { get; set; } = null!;

    public string ValuePath { get; set; } = null!;

    public Guid IsMissingId { get; set; }

    public virtual DocumentDataMissingAction IsMissing { get; set; } = null!;

    public virtual DocumentRule Rule { get; set; } = null!;
}
