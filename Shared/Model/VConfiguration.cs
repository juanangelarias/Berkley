using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class VConfiguration
{
    public Guid Id { get; set; }

    public Guid ConfigurationApplicationId { get; set; }

    public string Key { get; set; } = null!;

    public string? Value { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public Guid? ModifiedByUserId { get; set; }
}
