using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class RenewalRequest
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string BasisBondNumber { get; set; } = null!;

    public string RequestedBy { get; set; } = null!;

    public string? SendEmailTo { get; set; }

    public DateTime? RenewalStarted { get; set; }

    public string? DataUsed { get; set; }

    public DateTime? EmailSent { get; set; }

    public DateTime? RenewalCompleted { get; set; }

    public string? CancelReason { get; set; }

    public int PolMod { get; set; }

    public DateTime? UnderWriterNotified { get; set; }

    public Guid? RenewalRequestSourceId { get; set; }

    public DateTime? BondChangeReportUploaded { get; set; }

    public string? UploadError { get; set; }

    public DateTime? BondChangeReportEmailed { get; set; }

    public string? EmailError { get; set; }

    public string? BondChangeReportToEmail { get; set; }

    public string? AccountName { get; set; }
}
