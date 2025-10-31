using James.Shared.Model;

namespace JamesWebUI.Client.Classes;

public class AccountWatchDisplay: AccountWatch
{
    public string PreviousWatchStatus { get; set; } = "";

    public AccountWatchDisplay(AccountWatch watch)
    {
        Id = watch.Id;
        AccountNum = watch.AccountNum;
        WatchDate = watch.WatchDate;
        WatchStatus = watch.WatchStatus;
        Reason = watch.Reason;
        ActionPlan = watch.ActionPlan;
    }

    public AccountWatch GetAccountWatch()
    {
        return new AccountWatch
        {
            Id = Id,
            AccountNum = AccountNum,
            WatchDate = WatchDate,
            WatchStatus = WatchStatus,
            Reason = Reason,
            ActionPlan = ActionPlan
        };
    }
}