using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class CountryDm
{
    public string Code { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public bool Active { get; set; }

    public string PhoneCode { get; set; } = null!;

    public string PhoneMask { get; set; } = null!;

    public string PostalCodeMask { get; set; } = null!;

    public string? AddressFinalLineFormat { get; set; }

    public virtual ICollection<Insurer> Insurers { get; set; } = new List<Insurer>();

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
