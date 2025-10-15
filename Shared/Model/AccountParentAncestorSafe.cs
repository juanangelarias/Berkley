namespace James.Shared.Model;

public class AccountParentAncestorSafe
{
    public Guid? Id { get; set; }

    public string? AccountNum { get; set; }

    public Guid? ParentId { get; set; }

    public string? ParentAcctNumber { get; set; }

    public Guid? AncestorId { get; set; }

    public string? AncestorAccountNum { get; set; }
}