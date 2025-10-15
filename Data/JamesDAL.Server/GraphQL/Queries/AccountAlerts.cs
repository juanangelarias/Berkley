using HotChocolate.Authorization;
using James.Shared.Constants;
using James.Shared.Dto;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    private readonly Random _random = new(DateTime.Today.Millisecond);

    [Authorize]
    public async Task<AccountAlertPackageDto> GetAccountAlerts(int period, string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var startDate = GetStartDate(DateTime.Today, (AlertPeriod)period);
        
        var relatedAccountNumbers = await ctx.AccountParentAncestorSafe
            .Where(r=>r.AncestorAccountNum == accountNum)
            .Select(s=>s.AccountNum)
            .ToListAsync();

        var statusChangedAccounts = await ctx.AccountStatusLogs
            .Include(i => i.AccountNumNavigation)
            .Where(r => relatedAccountNumbers.Contains(r.AccountNumNavigation.AccountNum) && r.Effective >= startDate)
            .ToListAsync();

        var lostStatuses = new List<string> { "Lost", "Term. Comp.", "Term. Agent" };
        var lostAccounts = statusChangedAccounts
            .Where(r => lostStatuses.Contains(r.AccountStatus))
            .ToList();
        
        var newAccounts = statusChangedAccounts
            .Where(r => r.AccountStatus == "Active");
        
        // ToDo - Add new claims
        // ToDo - Add Messages Flag
        // ToDo - Add Expiration
        // ToDo - Add Submission
        
        // ToDo - Add Claims
        
        var notifications = new AccountAlertPackageDto();

        notifications.Alerts.AddRange(lostAccounts.Select(s=> new AccountAlertDto
        {
            Type = AccountAlertType.LostAccount,
            Date = s.Effective,
            Alert = $"Account: {s.AccountNumNavigation.AccountNum}",
            Link = $"{Links.Account}{s.AccountNumNavigation.AccountNum}"
        }));
        
        notifications.Alerts.AddRange(newAccounts.Select(s=>new AccountAlertDto
        {
            Type = AccountAlertType.NewAccount,
            Date = s.Effective,
            Alert = $"Account: {s.AccountNumNavigation.AccountNum}",
            Link = $"{Links.Account}{s.AccountNumNavigation.AccountNum}"
        }));
        
        notifications.Alerts = notifications.Alerts
            .OrderByDescending(o => o.Type)
            .ThenBy(t=>t.Date)
            .ToList();

        return notifications;
    }

    private static DateTime GetStartDate(DateTime endDate, AlertPeriod period)
    {
        var startDate = period switch
        {
            AlertPeriod.Last30Days => endDate.AddDays(-30),
            AlertPeriod.Last3Months => endDate.AddMonths(-3),
            AlertPeriod.Last6Months => endDate.AddMonths(-6),
            AlertPeriod.LastYear => endDate.AddYears(-1),
            _ => endDate.AddDays(-30)
        };
        return startDate;
    }

    private async Task<List<string>> GetRelatedAccounts(JamesDatabaseContext ctx, string prmAccountNum)
    {
        var accounts = await ctx.Accounts
            .Include(i => i.IdNavigation)
            .ThenInclude(t => t.ParentNavigation)
            .ThenInclude(t1 => t1!.AccountIdNavigation)
            .Select(s => new AccountControl
            {
                AccountNum = s.AccountNum,
                ParentAccountNum = s.IdNavigation.ParentNavigation!.AccountIdNavigation!.AccountNum
            })
            .ToListAsync();

        foreach (var account in accounts
                     .Where(account => account.AccountNum == account.ParentAccountNum))
        {
            account.ParentAccountNum = null;
        }

        var parentAccount = GetParentAccount(prmAccountNum, accounts);

        return GetChildren(parentAccount, accounts);
    }

    private List<string> GetChildren(string accountNum, List<AccountControl> accounts)
    {
        var relatedAccounts = new List<string>();
        var children = accounts
            .Where(a => a.ParentAccountNum == accountNum)
            .Select(a => a.AccountNum)
            .ToList();

        relatedAccounts.AddRange(children);

        foreach (var child in children)
        {
            relatedAccounts.AddRange(GetChildren(child, accounts));
        }

        return relatedAccounts;
    }

    private string GetParentAccount(string accountNum, List<AccountControl> accounts)
    {
        var acc = accounts.FirstOrDefault(a => a.AccountNum == accountNum);

        return acc?.ParentAccountNum == null
            ? accountNum
            : GetParentAccount(acc.ParentAccountNum, accounts);
    }

    private static DateTime GetRandomDate(DateTime startDate, DateTime endDate)
    {
        var random = new Random();
        var randomDate = startDate.AddDays(random.Next(0, (int)(endDate - startDate).TotalDays));
        return randomDate;
    }

    private static List<string> GetTexts()
    {
        return
        [
            "Lorem ipsum dolor sit amet consectetur adipiscing elit.",
            "Ex sapien vitae pellentesque sem placerat in id.",
            "Pretium tellus duis convallis tempus leo eu aenean.",
            "Urna tempor pulvinar vivamus fringilla lacus nec metus.",
            "Iaculis massa nisl malesuada lacinia integer nunc posuere.",
            "Semper vel class aptent taciti sociosqu ad litora.",
            "Conubia nostra inceptos himenaeos orci varius natoque penatibus.",
            "Dis parturient montes nascetur ridiculus mus donec rhoncus.",
            "Nulla molestie mattis scelerisque maximus eget fermentum odio.",
            "Purus est efficitur laoreet mauris pharetra vestibulum fusce."
        ];
    }

    class AccountControl
    {
        public string AccountNum { get; set; } = "";
        public string? ParentAccountNum { get; set; }
    }
}