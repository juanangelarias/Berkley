using HotChocolate.Authorization;
using James.Data.Server.Exceptions;
using James.Shared.Constants;
using James.Shared.Dto;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<AccountAlertPackageDto> GetAccountAlerts(int period, string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var start = DateTime.Now;
        
        var ctx = await contextFactory.CreateDbContextAsync();

        var startDate = GetStartDate(DateTime.Today, (AlertPeriod)period);
        
        var accountId = (await ctx.Accounts.FirstOrDefaultAsync(a => a.AccountNum == accountNum))?.Id;
        if (accountId == null)
            throw new NotFoundException("Account not found");
        
        var topParentId = (await ctx.VEntityTopParents.FirstOrDefaultAsync(f=>f.ChildId == accountId))?
            .ParentId;
        
        var parentAccountNum = (await ctx.Accounts.FirstOrDefaultAsync(a => a.Id == topParentId))?.AccountNum;
        if(parentAccountNum == null)
            throw new NotFoundException("Parent Account not found");
        
        var relatedAccountNumbers = await ctx.AccountParentAncestorSaves
            .Where(r=>r.AncestorAccountNum == parentAccountNum)
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
        var rnd = new Random(DateTime.Now.Millisecond);
        var claims = new List<AccountAlertClaimDto>();
        for (var i = 0; i < 3; i++)
        {
            claims.Add(new()
            {
                ClaimId = $"CL-{rnd.Next(100000, 999999):000000}",
                BondNumber = $"{rnd.Next(100000, 999999):000000}",
                ClassCode = $"{i:000}"
            });
        }
        var alerts = new List<AccountAlertDto>();
        alerts.AddRange(lostAccounts.Select(s=> new AccountAlertDto
        {
            Type = AccountAlertType.LostAccount,
            Date = s.Effective,
            Alert = $"Account: {s.AccountNumNavigation.AccountNum}",
            Link = $"{Links.Account}{s.AccountNumNavigation.AccountNum}"
        }));
        
        alerts.AddRange(newAccounts.Select(s=>new AccountAlertDto
        {
            Type = AccountAlertType.NewAccount,
            Date = s.Effective,
            Alert = $"Account: {s.AccountNumNavigation.AccountNum}",
            Link = $"{Links.Account}{s.AccountNumNavigation.AccountNum}"
        }));
        
        var notifications = new AccountAlertPackageDto
        {
            Alerts = alerts.OrderBy(o=>o.Type).ThenBy(t=>t.Date).ToList(),
            Claims = claims
        };

        var lapse = DateTime.Now - start;
        Console.WriteLine("*** *** *** *** ***");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine($"GetAccountAlerts took {lapse.TotalMilliseconds} ms");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("*** *** *** *** ***");
        
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
}