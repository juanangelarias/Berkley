using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class DocumentRule
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Condition { get; set; } = null!;

    public string OutputDefinition { get; set; } = null!;

    public string? ForEach { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DocumentDefinitionRule> DocumentDefinitionRules { get; set; } = new List<DocumentDefinitionRule>();

    public virtual ICollection<DocumentRuleReplacementMap> DocumentRuleReplacementMaps { get; set; } = new List<DocumentRuleReplacementMap>();
}
