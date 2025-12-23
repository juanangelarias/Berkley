using System.Web.Http;
using James.Shared.Dto;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<LineOfAuthorityLog>> GetLoaLogsByAccount(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var response = await ctx.LineOfAuthorityLogs
            .Where(l => l.AccountNum == accountNumber)
            .ToListAsync();

        return response;
    }

    [Authorize]
    public async Task<AccountLOAsDto> GetAccountLOAs(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx1 = await contextFactory.CreateDbContextAsync();
        var ctx2 = await contextFactory.CreateDbContextAsync();
        var ctx3 = await contextFactory.CreateDbContextAsync();

        var approvedLOAByAccountTask = GetApprovedLOAByAccount(accountNum, ctx1);
        var approvedAgencyLOAByAccountTask = GetApprovedAgencyLOAByAccount(accountNum, ctx2);
        var openBondsTotalTask = GetAccountOpenBondsTotal(accountNum, ctx3);
        await Task.WhenAll(approvedAgencyLOAByAccountTask, approvedLOAByAccountTask, openBondsTotalTask);

        var approvedLOAByAccount = approvedLOAByAccountTask.Result;
        var approvedAgencyLOAByAccount = approvedAgencyLOAByAccountTask.Result;
        var openBondsTotal = openBondsTotalTask.Result;

        var response = new AccountLOAsDto
        {
            AccountLOAs = approvedLOAByAccount,
            AgencyLOAs = approvedAgencyLOAByAccount,
            LOATotal = openBondsTotal
        };

        return response;
    }

    private async Task<List<AccountLOADetailDto>> GetApprovedLOAByAccount(string accountNum,
        JamesDatabaseContext ctx)
    {
        var data = await ctx.LineOfAuthorityLogs
            .OrderBy(o => o.AccountNum)
            .ThenBy(t => t.BondType)
            .ThenByDescending(t => t.Effective)
            .Where(r => r.AccountNum == accountNum && r.Status == "Approved")
            .Select(s => new AccountLOADetailDto
            {
                Aggregate = s.Loaaggregate,
                BondType = s.BondType,
                Effective = s.Effective,
                Expiration = s.Expiration,
                Single = s.Loasingle,
                Status = s.Status
            })
            .ToListAsync();

        var contract = data.FirstOrDefault(f => f.BondType == "Contract");
        var commercial = data.FirstOrDefault(f => f.BondType == "Commercial");

        var result = new List<AccountLOADetailDto>();
        if (contract != null)
            result.Add(contract);
        if (commercial != null)
            result.Add(commercial);

        return result;
    }

    private async Task<List<AccountLOADetailDto>> GetApprovedAgencyLOAByAccount(string accountNum,
        JamesDatabaseContext ctx)
    {
        var data = await ctx.AgencyLineOfAuthorityLogs
            .OrderBy(o => o.AccountNum)
            .ThenBy(t => t.BondType)
            .ThenByDescending(t => t.Effective)
            .Where(r => r.AccountNum == accountNum && r.Status == "Approved")
            .Select(s => new AccountLOADetailDto
            {
                Aggregate = s.Loaaggregate,
                BondType = s.BondType,
                Effective = s.Effective,
                Expiration = s.Expiration,
                Single = s.Loasingle,
                Status = s.Status
            })
            .ToListAsync();

        var contract = data.FirstOrDefault(f => f.BondType == "Contract");
        var commercial = data.FirstOrDefault(f => f.BondType == "Commercial");

        var result = new List<AccountLOADetailDto>();
        if (contract != null)
            result.Add(contract);
        if (commercial != null)
            result.Add(commercial);

        return result;
    }

    private async Task<int> GetAccountOpenBondsTotal(string accountNum, JamesDatabaseContext ctx)
    {
        var openBondsTransaccion = await ctx.BondTransactions
            .Include(i => i.BondNumberNavigation)
            .ThenInclude(i => i.BondType)
            .Where(r => r.AccountNum == accountNum &&
                        r.BondNumberNavigation.Status == "Open")
            .ToListAsync();

        var total = openBondsTransaccion
            .Where(r => r.Type == "Initial Premium")
            .ToList()
            .Sum(s => s.BondAmount);

        return total;
    }
}