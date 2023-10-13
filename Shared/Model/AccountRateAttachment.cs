using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class AccountRateAttachment
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid AccountRateId { get; set; }

    public string FileName { get; set; } = null!;

    public byte[] Attachment { get; set; } = null!;

    public virtual AccountRate AccountRate { get; set; } = null!;
}
