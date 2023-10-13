using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class ImagingTab
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string TabName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<ImagingCategoryTabDivision> ImagingCategoryTabDivisions { get; set; } = new List<ImagingCategoryTabDivision>();
}
