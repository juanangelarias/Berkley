using James.Shared;
using Microsoft.AspNetCore.Authorization;

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

        return true;
    }
}