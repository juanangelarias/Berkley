using System;
using System.Collections.Generic;

namespace James.Shared.Model;
//Generated for DB

public partial class NoteTypeDm
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public string NoteType { get; set; } = null!;
}
