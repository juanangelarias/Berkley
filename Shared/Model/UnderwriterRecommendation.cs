using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class UnderwriterRecommendation
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public string PostedBy { get; set; } = null!;

    public string Comments { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;
}
