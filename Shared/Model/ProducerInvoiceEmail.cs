using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ProducerInvoiceEmail
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public int Pnumber { get; set; }

    public string InvoiceEmail { get; set; } = null!;
}
