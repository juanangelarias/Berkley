using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ImagingTabType
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public Guid ImagingTabId { get; set; }

    public Guid ImagingTypeId { get; set; }

    public virtual ImagingTab ImagingTab { get; set; } = null!;

    public virtual ImagingType ImagingType { get; set; } = null!;
}
