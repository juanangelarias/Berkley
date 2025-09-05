namespace James.Shared.Model;

public partial class AccountWatch
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public DateTime WatchDate { get; set; }

    public string WatchStatus { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string ActionPlan { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual WatchStatusDm WatchStatusNavigation { get; set; } = null!;
}
