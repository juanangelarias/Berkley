using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class Indemnitor
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string AccountNum { get; set; } = null!;

    public DateOnly AgreementDate { get; set; }

    public string? AgreementType { get; set; }

    public string? AgreementForm { get; set; }

    public int? NetLiquidAssets { get; set; }

    public int? NetWorth { get; set; }

    public int? IndemnityAmount { get; set; }

    public bool SpouseIndemnitor { get; set; }

    public string? EncryptSpouseTaxId { get; set; }

    public string? Signatory { get; set; }

    public string? Title { get; set; }

    public virtual Account AccountNumNavigation { get; set; } = null!;

    public virtual LegalEntity IdNavigation { get; set; } = null!;
}
