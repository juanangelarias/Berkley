using System;

namespace James.Shared.Model;

public partial class Insurer
{
    public string FullName => IdNavigation?.FullName ?? string.Empty;
}
