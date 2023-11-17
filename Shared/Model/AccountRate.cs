using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace James.Shared.Model;
//Generated for DB

[Table("AccountRate", Schema ="dbo")]
public partial class AccountRate
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string CoNum { get; set; } = null!;

    public string? AgentContactInfo { get; set; }

    public string? CommissionRateCalculation { get; set; }

    public string? PremiumRateCalculation { get; set; }

    public string? SpecialProcessingInstructions { get; set; }

    public string? InvoiceEmail { get; set; }

    public bool IsParent { get; set; }

    public virtual ICollection<AccountRateAttachment> AccountRateAttachments { get; set; } = new List<AccountRateAttachment>();
}
