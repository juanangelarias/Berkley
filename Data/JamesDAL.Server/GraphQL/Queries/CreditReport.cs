using Microsoft.AspNetCore.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<CreditReportHistory>> GetCreditReport(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var creditReports = await ctx.CreditReportHistories
            .Where(c => c.AccountNum == accountNumber)
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.Pulled)
            .ToListAsync();

        return creditReports;
    }
    
    [Authorize]
    public async Task<CreditReportHistory?> GetLastCreditReport(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var creditReports = await ctx.CreditReportHistories
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.Pulled)
            .FirstOrDefaultAsync(c => c.AccountNum == accountNumber);

        return creditReports;
    }

    [Authorize]
    public async Task<List<CreditReportDm>> GetCreditReportAgencies(
        IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var creditReports = await ctx.CreditReportDms
            .OrderBy(o => o.CreditReport)
            .ToListAsync();
        return creditReports;
    }
}