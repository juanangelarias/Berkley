using James.Shared.Dto;
using Microsoft.AspNetCore.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<CreditReportDto> GetCreditReport(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var agenciesTask = GetCreditReportAgencies(contextFactory);
        var reportsTask = GetCreditReports(accountNumber, contextFactory);
        
        await Task.WhenAll(agenciesTask, reportsTask);

        var response = new CreditReportDto
        {
            CreditReportAgencies = agenciesTask.Result,
            CreditReports = reportsTask.Result
        };
        
        return response;
    }

    private async Task<List<CreditReportDm>> GetCreditReportAgencies(
        IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var creditReports = await ctx.CreditReportDms
            .ToListAsync();
        return creditReports;
    }

    private async Task<List<CreditReportHistory>> GetCreditReports(string accountNumber,
        IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var creditReports = await ctx.CreditReportHistories
            .Where(c => c.AccountNum == accountNumber)
            .ToListAsync();
        
        return creditReports;
    }
}