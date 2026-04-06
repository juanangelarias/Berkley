using James.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize(Policy = "InRoleCanSetAccountProgram")]
    public async Task<bool> SetAccountProgram(Guid programId, string accountNum, DateTime effective,
        DateTime? expiration, int single, int aggregate, string? comments, Guid statusId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var userName = await userShared.GetUserName();

        //ToDo: Review after we move to the new way to authenticate users that have this issue
        //ToDo: (active directory account different than the email)

        var employee = await ctx.Employees
                           .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == userName) ??
                       await ctx.Employees
                           .FirstOrDefaultAsync(f => f.Email!.StartsWith(userName));

        if (employee == null)
            throw new GraphQLException("User not found in employee table.");

        AccountProgramStatusHistory CreateStatusLog(AccountProgramStatusHistory? lastLog) => new()
        {
            Id = Guid.NewGuid(),
            AccountProgramId = programId,
            AccountNum = accountNum,
            OldStatus = lastLog?.NewStatus,
            NewStatus = statusId,
            StatusDate = DateTime.Now,
            OldSingle = lastLog?.NewSingle,
            NewSingle = single,
            OldAggregate = lastLog?.NewAggregate,
            NewAggregate = aggregate,
            StatusChangeBy = employee.Id
        };

        var existing = await ctx.AccountPrograms
            .FirstOrDefaultAsync(f => f.Id == programId);
        if (existing != null)
        {
            existing.Effective = effective;
            existing.Expiration = expiration ?? new DateTime(9999, 12, 31);
            existing.Single = single;
            existing.Aggregate = aggregate;
            existing.Modified = DateTime.Now;
            existing.CreatedBy = employee.Id;
            existing.Comments = comments;

            var lastLog = await ctx.AccountProgramStatusHistories
                .OrderBy(o => o.AccountProgramId)
                .ThenByDescending(t => t.Created)
                .FirstOrDefaultAsync(f => f.AccountProgramId == programId);

            var newLog = CreateStatusLog(lastLog);
            ctx.AccountProgramStatusHistories.Add(newLog);
            ctx.Update(existing);
        }
        else
        {
            var program = new AccountProgram
            {
                Id = programId,
                AccountNum = accountNum,
                Effective = effective,
                Expiration = expiration ?? new DateTime(9999, 12, 31),
                Single = single,
                Aggregate = aggregate,
                StatusId = statusId,
                Created = DateTime.Now,
                CreatedBy = employee.Id,
                Modified = DateTime.Now,
                ApprovedBy = null,
                ApprovedDate = null,
                Comments = comments
            };
            ctx.AccountPrograms.Add(program);
            ctx.AccountProgramStatusHistories.Add(CreateStatusLog(null));
        }

        await ctx.SaveChangesAsync();
        return true;
    }

    [Authorize(Policy = "InRoleCanDeleteAccountProgram")]
    public async Task<bool> DeleteAccountProgram(Guid accountProgramId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var record = await ctx.AccountPrograms
            .FirstOrDefaultAsync(f => f.Id == accountProgramId);

        if (record == null)
            throw new GraphQLException("Account program status dms not found.");

        var history = await ctx.AccountProgramStatusHistories
            .Where(f => f.AccountProgramId == accountProgramId)
            .ToListAsync();

        ctx.AccountProgramStatusHistories.RemoveRange(history);
        ctx.AccountPrograms.Remove(record);

        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize(Policy = "InRoleCanSetAccountProgram")]
    public async Task<bool> AccountProgramChangeStatus(Guid accountProgramId, string newStatusTxt,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IUserShared userShared)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        //ToDo: Review after we move to the new way to authenticate users that have this issue
        //ToDo: (active directory account different than the email)
        
        var user = await userShared.GetCurrentUser();
        var employee = await ctx.Employees
                           .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == user.Username) ??
                       await ctx.Employees
                           .FirstOrDefaultAsync(f => f.Email!.StartsWith(user.Username));

        if (employee == null)
            throw new GraphQLException("User not found in employee table.");

        var newStatus = ctx.AccountProgramStatusDms
            .FirstOrDefault(f => f.Description.ToUpper() == newStatusTxt.ToUpper());

        if (newStatus == null)
            throw new GraphQLException($"'{newStatusTxt}' status not found");

        var program = await ctx.AccountPrograms.FindAsync(accountProgramId);
        if (program == null)
            throw new GraphQLException("Account program not found");

        var lastLog = await ctx.AccountProgramStatusHistories
            .OrderBy(o => o.AccountProgramId)
            .ThenByDescending(t => t.Created)
            .FirstOrDefaultAsync(f => f.AccountProgramId == accountProgramId);

        var newLog = new AccountProgramStatusHistory
        {
            Id = Guid.NewGuid(),
            AccountProgramId = program.Id,
            AccountNum = program.AccountNum,
            OldStatus = lastLog?.NewStatus,
            NewStatus = newStatus.Id,
            StatusDate = DateTime.Now,
            OldSingle = lastLog?.NewSingle,
            NewSingle = program.Single,
            OldAggregate = lastLog?.NewAggregate,
            NewAggregate = program.Aggregate,
            StatusChangeBy = employee.Id
        };

        ctx.AccountProgramStatusHistories.Add(newLog);
        program.StatusId = newStatus.Id;
        program.Modified = DateTime.Now;
        program.ApprovedDate = DateTime.Now;

        if (newStatusTxt.ToUpper() == "APPROVED")
        {
            program.ApprovedBy = employee.Id;
            program.ApprovedDate = DateTime.Now;
        }

        await ctx.SaveChangesAsync();

        if (newStatusTxt.ToUpper() == "APPROVAL REQUESTED")
        {
            // ToDo: Send email to approver
        }

        if (newStatusTxt.ToUpper() == "APPROVED")
        {
            // ToDo: Send approval email to the requester
        }

        if (newStatusTxt.ToUpper() == "DECLINED")
        {
            // ToDo: Send decline email to the requester
        }

        return true;
    }

    [Authorize]
    public async Task<bool> SetLoaLog(Guid id, string accountNum, DateTime effective, DateTime expiration,
        int loaSingle, int loaAggregate, string comments, string status, string division, string bondType,
        string conditions, bool homeOfficeApproved, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var employee = await GetEmployee(contextAccessor, ctx);

        var loa = await ctx.LineOfAuthorityLogs.FindAsync(id);
        if (loa == null)
        {
            loa = new()
            {
                Id = id,
                AccountNum = accountNum,
                Effective = effective,
                Expiration = expiration,
                Loasingle = loaSingle,
                Loaaggregate = loaAggregate,
                Comments = comments,
                Status = status,
                Division = division,
                BondType = bondType,
                Conditions = conditions,
                HomeOfficeApproved = homeOfficeApproved,
                CreatedBy = employee!.Id
            };

            ctx.LineOfAuthorityLogs.Add(loa);
        }
        else
        {
            loa.AccountNum = accountNum;
            loa.Effective = effective;
            loa.Expiration = expiration;
            loa.Loasingle = loaSingle;
            loa.Loaaggregate = loaAggregate;
            loa.Comments = comments;
            loa.Status = status;
            loa.Division = division;
            loa.BondType = bondType;
            loa.Conditions = conditions;
            loa.HomeOfficeApproved = homeOfficeApproved;
            loa.CreatedBy = employee!.Id;
        }

        // ToDo: add functionality to add the LoaHistory when creating or updating a LOA

        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> LoaLogDelete(Guid accountLoaId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loa = await ctx.LineOfAuthorityLogs.FindAsync(accountLoaId);
        if (loa == null)
            return false;

        ctx.LineOfAuthorityLogs.Remove(loa);
        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> LoaLogChangeStatus(Guid accountLoaId, string newStatusTxt,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loa = await ctx.LineOfAuthorityLogs.FindAsync(accountLoaId);
        if (loa == null)
            throw new GraphQLException("Line of authority log not found");

        var employee = await GetEmployee(contextAccessor, ctx);

        // ToDo: add functionality to change the LoaHistory status when changing the LOA status

        loa.Status = newStatusTxt;
        loa.Modified = DateTime.Now;
        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> SetAgencyLoa(Guid id, string agencyNumber, string accountNum,
        DateTime effective, DateTime expiration, int loaSinge, int loaAggregate, string comments, string division,
        string bondType, string conditions, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var employee = await GetEmployee(contextAccessor, ctx);

        var loa = await ctx.AgencyLineOfAuthorityLogs.FindAsync(id);
        if (loa == null)
        {
            loa = new()
            {
                Id = id,
                AgencyNumber = agencyNumber,
                AccountNum = accountNum,
                Effective = effective,
                Expiration = expiration,
                Loasingle = loaSinge,
                Loaaggregate = loaAggregate,
                Comments = comments,
                Division = division,
                BondType = bondType,
                CreatedBy = employee!.Id,
                Conditions = conditions
            };

            ctx.AgencyLineOfAuthorityLogs.Add(loa);
        }
        else
        {
            loa.AgencyNumber = agencyNumber;
            loa.AccountNum = accountNum;
            loa.Effective = effective;
            loa.Expiration = expiration;
            loa.Loasingle = loaSinge;
            loa.Loaaggregate = loaAggregate;
            loa.Comments = comments;
            loa.Division = division;
            loa.BondType = bondType;
            loa.Conditions = conditions;
        }

        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> AgencyLoaDelete(Guid agencyLoaId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var loa = await ctx.AgencyLineOfAuthorityLogs.FindAsync(agencyLoaId);
        if (loa == null)
            throw new GraphQLException("Agency line of authority log not found");

        ctx.AgencyLineOfAuthorityLogs.Remove(loa);
        await ctx.SaveChangesAsync();

        return true;
    }

    private static async Task<Employee?> GetEmployee(IHttpContextAccessor contextAccessor, JamesDatabaseContext ctx)
    {
        var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
        if (null == username)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        var employee = await ctx.Employees
                           .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == username) ??
                       await ctx.Employees
                           .FirstOrDefaultAsync(f => f.Email.StartsWith(username));

        return employee ?? throw new GraphQLException("User not found in employee table.");
    }
}