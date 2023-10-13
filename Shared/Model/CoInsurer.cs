using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CoInsurer
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BondNumber { get; set; } = null!;

    public Guid InsurerId { get; set; }

    public double? Percentage { get; set; }

    public string? Comments { get; set; }
}
