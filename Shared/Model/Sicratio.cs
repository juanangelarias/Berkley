using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Sicratio
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Code { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public double UpperQuartileStated { get; set; }

    public double MedianStated { get; set; }

    public double? LowerQuartileStated { get; set; }

    public double UpperQuartileAllowed { get; set; }

    public double MedianAllowed { get; set; }

    public double? LowerQuartileAllowed { get; set; }

    public int SampleSize { get; set; }

    public virtual Sic CodeNavigation { get; set; } = null!;

    public virtual SicratioTypeDm TypeNavigation { get; set; } = null!;
}
