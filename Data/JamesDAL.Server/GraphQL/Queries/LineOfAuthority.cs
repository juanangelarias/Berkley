using James.Shared.Dto;
using System.Web.Http;
using James.Data.Server.Exceptions;
using James.Shared;
using James.Shared.Constants;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<AccountLOADto>> GetLoaLogsByAccount(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var response = await ctx.LineOfAuthorityLogs
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.ApprovedByNavigation)
            .Where(l => l.AccountNum == accountNumber)
            .Select(s => new AccountLOADto
            {
                Id = s.Id,
                Created = s.Created,
                CreatedByName = s.CreatedBy != null
                    ? s.CreatedByNavigation!.FullName
                    : null,
                ApprovedByName = s.ApprovedBy != null
                    ? s.ApprovedByNavigation!.FullName
                    : null,
                AccountNum = s.AccountNum,
                SequenceNumber = s.SequenceNumber,
                Effective = s.Effective,
                Expiration = s.Expiration,
                LoaSingle = s.Loasingle,
                LoaAggregate = s.Loaaggregate,
                Division = s.Division,
                BondType = s.BondType,
                HomeOfficeApproved = s.HomeOfficeApproved,
                Comments = s.Comments,
                Conditions = s.Conditions,
                Status = s.Status,
                ReasonId = s.ReasonId
            })
            .OrderBy(o=>o.AccountNum)
            .ThenByDescending(o=>o.Effective)
            .ToListAsync();

        return response;
    }

    public async Task<AccountLOADto?> GetLoaLogById(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var response = await ctx.LineOfAuthorityLogs
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.ApprovedByNavigation)
            .Select(s => new AccountLOADto
            {
                Id = s.Id,
                Created = s.Created,
                CreatedByName = s.CreatedBy != null
                    ? s.CreatedByNavigation!.FullName
                    : null,
                ApprovedByName = s.ApprovedBy != null
                    ? s.ApprovedByNavigation!.FullName
                    : null,
                AccountNum = s.AccountNum,
                SequenceNumber = s.SequenceNumber,
                Effective = s.Effective,
                Expiration = s.Expiration,
                LoaSingle = s.Loasingle,
                LoaAggregate = s.Loaaggregate,
                Division = s.Division,
                BondType = s.BondType,
                HomeOfficeApproved = s.HomeOfficeApproved,
                Comments = s.Comments,
                Conditions = s.Conditions,
                Status = s.Status
            })
            .FirstOrDefaultAsync(l => l.Id == id);

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
            .Where(r => r.AccountNum == accountNum && r.Status == LOAStatus.Approved)
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

        var contract = data.FirstOrDefault(f => f.BondType == BondType.Contract.ToString());
        var commercial = data.FirstOrDefault(f => f.BondType == BondType.Commercial.ToString());

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

        var contract = data.FirstOrDefault(f => f.BondType == BondType.Contract.ToString());
        var commercial = data.FirstOrDefault(f => f.BondType == BondType.Commercial.ToString());

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
            .Include(i => i.AccountProgramStatusHistories)
            .ThenInclude(i => i.StatusChangeByNavigation)
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.ApprovedByNavigation)
            .Where(r => r.AccountNum == accountNum)
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.Effective)
            .ToListAsync();

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
                CreatedByName = program.CreatedByNavigation.FullName,
                ApprovedByName = program.ApprovedByNavigation?.FullName ?? "",
                ApprovedDate = program.ApprovedDate
            };
            foreach (var hst in program.AccountProgramStatusHistories.OrderByDescending(o => o.Created))
            {
                prg.Logs.Add(new()
                {
                    Id = hst.Id,
                    AccountNum = hst.AccountNum,
                    NewStatus = hst.NewStatusNavigation.Description,
                    OldStatus = hst.OldStatusNavigation?.Description,
                    StatusDate = hst.StatusDate,
                    StatusChangeByFullName = hst.StatusChangeByNavigation.FullName,
                    OldSingle = hst.OldSingle,
                    NewSingle = hst.NewSingle,
                    OldAggregate = hst.OldAggregate,
                    NewAggregate = hst.NewAggregate
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
            .Include(i => i.AccountProgramStatusHistories)
            .ThenInclude(i => i.StatusChangeByNavigation)
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.ApprovedByNavigation)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (program == null)
            throw new GraphQLException("Account program not found");

        var prg = new AccountProgramDto
        {
            Id = program.Id,
            Effective = program.Effective,
            Expiration = program.Expiration,
            Single = program.Single,
            Aggregate = program.Aggregate,
            StatusId = program.StatusId,
            Status = program.Status.Description,
            CreatedByName = program.CreatedByNavigation.FullName,
            ApprovedByName = program.ApprovedByNavigation?.FullName ?? "",
            ApprovedDate = program.ApprovedDate
        };
        foreach (var hst in program.AccountProgramStatusHistories.OrderByDescending(o => o.Created))
        {
            prg.Logs.Add(new()
            {
                Id = hst.Id,
                NewStatus = hst.NewStatusNavigation.Description,
                OldStatus = hst.OldStatusNavigation?.Description,
                StatusDate = hst.StatusDate,
                StatusChangeByFullName = hst.StatusChangeByNavigation.FullName,
                OldSingle = hst.OldSingle,
                NewSingle = hst.NewSingle,
                OldAggregate = hst.OldAggregate,
                NewAggregate = hst.NewAggregate
            });
        }

        return prg;
    }

    [Authorize]
    public async Task<List<AccountAgencyLOADto>> GetAccountAgencyLOA(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var data = await ctx.AgencyLineOfAuthorityLogs
            .Where(r => r.AccountNum == accountNum)
            .Select(s => new AccountAgencyLOADto
            {
                Id = s.Id,
                AccountNum = s.AccountNum,
                AgencyNumber = s.AgencyNumber,
                SequenceNumber = s.SequenceNumber,
                Effective = s.Effective,
                Expiration = s.Expiration,
                LoaSingle = s.Loasingle,
                LoaAggregate = s.Loaaggregate,
                Division = s.Division,
                BondType = s.BondType,
                Created = s.Created,
                CreatedById = s.CreatedBy,
                CreatedByName = s.CreatedBy == null ? null : s.CreatedByNavigation!.FullName
            })
            .ToListAsync();

        return data;
    }

    [Authorize]
    public async Task<AccountAgencyLOADto?> GetAccountAgencyLOAById(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var data = await ctx.AgencyLineOfAuthorityLogs
            .Select(s => new AccountAgencyLOADto
            {
                Id = s.Id,
                AccountNum = s.AccountNum,
                AgencyNumber = s.AgencyNumber,
                SequenceNumber = s.SequenceNumber,
                Effective = s.Effective,
                Expiration = s.Expiration,
                LoaSingle = s.Loasingle,
                LoaAggregate = s.Loaaggregate,
                Division = s.Division,
                BondType = s.BondType,
                Created = s.Created,
                CreatedById = s.CreatedBy,
                CreatedByName = s.CreatedBy == null ? null : s.CreatedByNavigation!.FullName
            })
            .FirstOrDefaultAsync(r => r.Id == id);

        return data;
    }

    [Authorize]
    public async Task<List<UserLineOfAuthority>> GetUserLOAByDivision(string division,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loggedUser = await userShared.GetUserName();
        if (string.IsNullOrEmpty(loggedUser))
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        var employee = await ctx.Employees
            .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == loggedUser) ?? await ctx.Employees
            .FirstOrDefaultAsync(f => f.Email!.StartsWith(loggedUser));

        if (employee == null)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        return await ctx.UserLineOfAuthorities
            .Where(f => f.UserId == employee.Id && f.DivisionCode == division)
            .ToListAsync();
    }

    // Reason - Renewals
    [Authorize]
    public async Task<List<LineOfAuthorityReason>> GetLOAReasonByAccount(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var reasons = await ctx.LineOfAuthorityReasons
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.LineOfAuthorityLogs)
            .ThenInclude(i => i.CreatedByNavigation)
            .Include(i => i.LineOfAuthorityLogs)
            .ThenInclude(i => i.ApprovedByNavigation)
            .Where(r => r.AccountNum == accountNum)
            .OrderBy(o => o.AccountNum)
            .ThenByDescending(o => o.Created)
            .ToListAsync();

        return reasons;
    }

    [Authorize]
    public async Task<LineOfAuthorityReason> GetLOAReasonById(Guid reasonId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var reason = await ctx.LineOfAuthorityReasons
            .Include(i => i.CreatedByNavigation)
            .Include(i => i.LineOfAuthorityLogs)
            .ThenInclude(i => i.CreatedByNavigation)
            .Include(i => i.LineOfAuthorityLogs)
            .ThenInclude(i => i.ApprovedByNavigation)
            .FirstOrDefaultAsync(r => r.Id == reasonId);

        return reason ?? throw new NotFoundException("Line of Authority Reason not found");
    }

    [Authorize]
    public async Task<List<string>> GetLoaRenewalFormsAvailableByAccount(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var forms = new List<string> { LOARenewalType.Rapid, LOARenewalType.Short, LOARenewalType.Standard };
        
        // ToDo: Logic to determine what forms will be available for this account

        return forms;
    }
}