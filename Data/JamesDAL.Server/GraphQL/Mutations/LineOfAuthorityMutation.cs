using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> SetAccountProgram(Guid programId, string accountNum, DateTime effective,
        DateTime? expiration, int single, int aggregate, string? comments, Guid statusId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
        if (null == username)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        var employee = (await ctx.Employees
            .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == username));

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

        var existent = await ctx.AccountPrograms
            .FirstOrDefaultAsync(f => f.Id == programId);
        if (existent != null)
        {
            existent.Effective = effective;
            existent.Expiration = expiration ?? new DateTime(9999, 12, 31);
            existent.Single = single;
            existent.Aggregate = aggregate;
            existent.Modified = DateTime.Now;
            existent.CreatedBy = employee.Id;
            existent.Comments = comments;

            var lastLog = await ctx.AccountProgramStatusHistories
                .OrderBy(o => o.AccountProgramId)
                .ThenByDescending(t => t.Created)
                .FirstOrDefaultAsync(f => f.AccountProgramId == programId);

            var newLog = CreateStatusLog(lastLog);
            ctx.AccountProgramStatusHistories.Add(newLog);
            ctx.Update(existent);
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

    [Authorize]
    public async Task<bool> DeleteAccountProgram(Guid accountProgramId, 
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var record = await ctx.AccountPrograms
            .FirstOrDefaultAsync(f => f.Id == accountProgramId);
        
        if(record == null)
            throw new GraphQLException("Account program status dms not found.");

        var history = await ctx.AccountProgramStatusHistories
            .Where(f => f.AccountProgramId == accountProgramId)
            .ToListAsync();
        
        ctx.AccountProgramStatusHistories.RemoveRange(history);
        ctx.AccountPrograms.Remove(record);
        
        await ctx.SaveChangesAsync();
        
        return true;
    }

    [Authorize]
    public async Task<bool> AccountProgramChangeStatus(Guid accountProgramId, string newStatusTxt,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
        if (null == username)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        var employee = (await ctx.Employees
            .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == username));
        
        if(employee == null)
            throw new GraphQLException("User not found in employee table.");

        var newStatus = ctx.AccountProgramStatusDms
            .FirstOrDefault(f => f.Description.ToUpper() == newStatusTxt.ToUpper());
        
        if(newStatus == null)
            throw new GraphQLException($"'{newStatusTxt}' status not found");
        
        var program = await ctx.AccountPrograms.FindAsync(accountProgramId);
        if(program == null)
            throw new GraphQLException("Account program not found");
        
        var lastLog = await ctx.AccountProgramStatusHistories
            .OrderBy(o=>o.AccountProgramId)
            .ThenByDescending(t=>t.Created)
            .FirstOrDefaultAsync(f=>f.AccountProgramId == accountProgramId);

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