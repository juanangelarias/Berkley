using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class BookRatio
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public double LowerQuartileStated { get; set; }

    public double MedianStated { get; set; }

    public double UpperQuartileStated { get; set; }

    public double LowerQuartileAllowed { get; set; }

    public double MedianAllowed { get; set; }

    public double UpperQuartileAllowed { get; set; }
}
