using James.Shared.Dto;
using System.Web.Http;

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
        var ctx = await contextFactory.CreateDbContextAsync();
        var account = await ctx.Accounts.SingleOrDefaultAsync(a => a.AccountNum == accountNum);
        if (account == null)
            throw new GraphQLException($"No account exists with account number {accountNum}");

        var parentAccountId = await GetParent(account.Id, contextFactory);
        if (parentAccountId == null)
            throw new GraphQLException($"No parent account exists for account {accountNum}");

        var parentAccountNum = (await ctx.Accounts.FirstOrDefaultAsync(a => a.Id == parentAccountId))?.AccountNum;
        if (parentAccountNum == null)
            throw new GraphQLException($"No parent account exists for account {accountNum}`");

        var ctx1 = await contextFactory.CreateDbContextAsync();
        var ctx2 = await contextFactory.CreateDbContextAsync();

        var approvedLOAByAccountTask = GetApprovedLOAByAccount(parentAccountNum, ctx1);
        var agencyLOAByAccountTask = GetAgencyLOAByAccount(parentAccountNum, ctx2);
        var openBondsTotalTask = GetAccountOpenBondsTotal(parentAccountId.Value, contextFactory);
        await Task.WhenAll(agencyLOAByAccountTask, approvedLOAByAccountTask, openBondsTotalTask);

        var approvedLOAByAccount = approvedLOAByAccountTask.Result;
        var agencyLOAByAccount = agencyLOAByAccountTask.Result;
        var openBondsTotal = openBondsTotalTask.Result;

        var response = new AccountLOAsDto
        {
            AccountLOAs = approvedLOAByAccount,
            AgencyLOAs = agencyLOAByAccount,
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

    private async Task<List<AccountLOADetailDto>> GetAgencyLOAByAccount(string accountNum,
        JamesDatabaseContext ctx)
    {
        var data = await ctx.AgencyLineOfAuthorityLogs
            .OrderBy(o => o.AccountNum)
            .ThenBy(t => t.BondType)
            .ThenByDescending(t => t.Effective)
            .Where(r => r.AccountNum == accountNum)
            .Select(s => new AccountLOADetailDto
            {
                Aggregate = s.Loaaggregate,
                BondType = s.BondType,
                Effective = s.Effective,
                Expiration = s.Expiration,
                Single = s.Loasingle
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

    private async Task<int> GetAccountOpenBondsTotal(Guid accountId, IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var accountNum = ctx.Accounts.FirstOrDefault(f => f.Id == accountId)?.AccountNum;
        if (accountNum == null)
            throw new GraphQLException($"No account exists with id {accountId}");

        var relatedAccounts = await ctx.AccountChildren
            .FromSqlInterpolated($"SELECT * FROM dbo.fnGetAllRelatedAccounts({accountNum})")
            .Select(s => s.AccountNum)
            .ToListAsync();
        //await GetRelatedAccounts(accountId, false, contextFactory);

        var openBondsTransaccion = await ctx.BondTransactions
            .Include(i => i.BondNumberNavigation)
            .ThenInclude(i => i.BondType)
            .Where(r => relatedAccounts.Contains(r.AccountNum) &&
                        r.BondNumberNavigation.Status == "Open")
            .ToListAsync();

        var total = openBondsTransaccion
            .Where(r => r.Type == "Initial Premium")
            .ToList()
            .Sum(s => s.BondAmount);

        return total;
    }
}