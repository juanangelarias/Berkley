using James.Shared.Dto;
using System.Web.Http;
using James.Shared;

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

    private async Task<int> GetAccountOpenBondsTotal(Guid accountId,
        IDbContextFactory<JamesDatabaseContext> contextFactory)
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

    [Authorize]
    public async Task<List<AccountProgramDto>> GetAccountPrograms(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var data = await ctx.AccountPrograms
            .Include(i => i.Status)
            .Include(i => i.AccountProgramStatusHistories)
            .ThenInclude(i => i.NewStatusNavigation)
            .Include(i => i.AccountProgramStatusHistories)
            .ThenInclude(i => i.OldStatusNavigation)
            // ToDo: Add Include to Employee (StatusChangeByNavigation)
            .Where(r => r.AccountNum == accountNum)
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.Effective)
            .ToListAsync();

        var employees = await ctx.Employees
            .ToListAsync();
        var createdByName = employees.FirstOrDefault(e => e.Id == data.FirstOrDefault()?.CreatedBy)?.FullName;
        var approvedByName = employees.FirstOrDefault(e => e.Id == data.FirstOrDefault()?.ApprovedBy)?.FullName;

        var result = new List<AccountProgramDto>();
        foreach (var program in data)
        {
            var prg = new AccountProgramDto
            {
                Id = program.Id,
                AccountNum = program.AccountNum,
                RequireExpiration = true, // ToDo: To be changed
                Effective = program.Effective,
                Expiration = program.Expiration,
                Single = program.Single,
                Aggregate = program.Aggregate,
                StatusId = program.StatusId,
                Status = program.Status.Description,
                CreatedById = program.CreatedBy,
                CreatedByName = createdByName ?? "",
                ApprovedById = program.ApprovedBy,
                ApprovedByName = approvedByName,
                ApprovedDate = program.ApprovedDate
            };
            foreach (var hst in program.AccountProgramStatusHistories.OrderByDescending(o => o.Created))
            {
                prg.Logs.Add(new AccountProgramStatusLogDto
                {
                    Id = hst.Id,
                    AccountNum = hst.AccountNum,
                    NewStatusId = hst.NewStatus,
                    NewStatus = hst.NewStatusNavigation.Description,
                    OldStatusId = hst.OldStatus,
                    OldStatus = hst.OldStatusNavigation?.Description,
                    StatusDate = hst.StatusDate,
                    StatusChangeBy = hst.StatusChangeBy,
                    StatusChangeByFullName = employees
                        .FirstOrDefault(e => e.Id == hst.StatusChangeBy)?
                        .FullName ?? "",
                    OldSingle = hst.OldSingle,
                    NewSingle = hst.NewSingle,
                    OldAggregate = hst.OldAggregate,
                    NewAggregate = hst.NewAggregate,
                });
            }

            result.Add(prg);
        }

        return result;
    }

    [Authorize]
    public async Task<AccountProgramDto> GetAccountProgramById(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var program = await ctx.AccountPrograms
            .Include(i => i.Status)
            .Include(i => i.AccountProgramStatusHistories)
            .ThenInclude(i => i.NewStatusNavigation)
            .Include(i => i.AccountProgramStatusHistories)
            .ThenInclude(i => i.OldStatusNavigation)
            // ToDo: Add Include to Employee (StatusChangeByNavigation)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (program == null)
            throw new GraphQLException("Account program not found");

        var employees = await ctx.Employees
            .ToListAsync();

        var createdByName = employees.FirstOrDefault(e => e.Id == program.CreatedBy)?.FullName;
        var approvedByName = employees.FirstOrDefault(e => e.Id == program.ApprovedBy)?.FullName;

        var prg = new AccountProgramDto
        {
            Id = program.Id,
            Effective = program.Effective,
            Expiration = program.Expiration,
            Single = program.Single,
            Aggregate = program.Aggregate,
            StatusId = program.StatusId,
            Status = program.Status.Description,
            CreatedById = program.CreatedBy,
            CreatedByName = createdByName ?? "",
            ApprovedById = program.ApprovedBy,
            ApprovedByName = approvedByName,
            ApprovedDate = program.ApprovedDate
        };
        foreach (var hst in program.AccountProgramStatusHistories.OrderByDescending(o => o.Created))
        {
            prg.Logs.Add(new AccountProgramStatusLogDto
            {
                Id = hst.Id,
                NewStatusId = hst.NewStatus,
                NewStatus = hst.NewStatusNavigation.Description,
                OldStatusId = hst.OldStatus,
                OldStatus = hst.OldStatusNavigation?.Description,
                StatusDate = hst.StatusDate,
                StatusChangeBy = hst.StatusChangeBy,
                StatusChangeByFullName = employees
                    .FirstOrDefault(e => e.Id == hst.StatusChangeBy)?
                    .FullName ?? "",
                OldSingle = hst.OldSingle,
                NewSingle = hst.NewSingle,
                OldAggregate = hst.OldAggregate,
                NewAggregate = hst.NewAggregate,
            });
        }

        return prg;
    }
    
    [HotChocolate.Authorization.Authorize]
    public async Task<List<UserLineOfAuthority>> GetUserLOAByDivision(string division,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loggedUser = await userShared.GetCurrentUser();
        if(loggedUser == null)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");
        
        var employee = await ctx.Employees
            .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == loggedUser.Username) ?? await ctx.Employees
            .FirstOrDefaultAsync(f => f.Email!.StartsWith(loggedUser.Username));

        if(employee == null)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");
        
        return await ctx.UserLineOfAuthorities
            .Where(f => f.UserId == employee.Id && f.DivisionCode == division)
            .ToListAsync();
    }
}