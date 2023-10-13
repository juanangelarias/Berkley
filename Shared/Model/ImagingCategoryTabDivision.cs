using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ImagingCategoryTabDivision
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string Category { get; set; } = null!;

    public string TabName { get; set; } = null!;

    public string DivisionCode { get; set; } = null!;

    public virtual ImagingCategory CategoryNavigation { get; set; } = null!;

    public virtual DivisionDm DivisionCodeNavigation { get; set; } = null!;

    public virtual ImagingTab TabNameNavigation { get; set; } = null!;
}
