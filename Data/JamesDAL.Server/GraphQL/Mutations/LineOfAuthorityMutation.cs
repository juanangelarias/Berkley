using James.Shared.Dto;
using Microsoft.AspNetCore.Authorization;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> CreateAccountProgram(AccountProgramDto input, 
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var logInput = input.Logs.FirstOrDefault();
        if(logInput == null)
            throw new GraphQLException("The input must contain at least one log entry");
        
        var program = new AccountProgram
        {
            Id = input.Id,
            AccountNum = input.AccountNum,
            Effective = input.Effective,
            Expiration = input.Expiration,
            Single = input.Single,
            Aggregate = input.Aggregate,
            StatusId = input.StatusId,
            CreatedBy = input.CreatedBy,
            ApprovedBy = null,
            ApprovedDate = null
        };
        
        var log = new AccountProgramStatusHistory
        {
            Id = logInput.Id,
            AccountProgramId = input.Id,
            AccountNum = input.AccountNum,
            OldStatus = null,
            NewStatus = logInput.NewStatusId,
            StatusDate = logInput.StatusDate,
            OldSingle = null,
            NewSingle = logInput.NewSingle,
            OldAggregate = null,
            NewAggregate = logInput.NewAggregate,
            StatusChangeBy = logInput.StatusChangeBy
        };
        
        var ctx = await contextFactory.CreateDbContextAsync();
        ctx.AccountPrograms.Add(program);
        ctx.AccountProgramStatusHistories.Add(log);
        await ctx.SaveChangesAsync();
        
        return true;
    }

    [Authorize]
    public async Task<bool> AccountProgramChangeStatus(Guid accountProgramId, Guid employeeId, string newStatusTxt,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

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
            OldStatus = lastLog!.NewStatus,
            NewStatus = newStatus.Id,
            StatusDate = DateTime.Now,
            OldSingle = lastLog!.NewSingle,
            NewSingle = program.Single,
            OldAggregate = lastLog!.NewAggregate,
            NewAggregate = program.Aggregate,
            StatusChangeBy = employeeId
        };
        
        ctx.AccountProgramStatusHistories.Add(newLog);
        program.StatusId = newStatus.Id;
        program.ApprovedDate = DateTime.Now;

        if (newStatusTxt.ToUpper() == "APPROVED")
        {
            program.ApprovedBy = "employeeId";
            program.ApprovedDate = DateTime.Now;
        }

        await ctx.SaveChangesAsync();
        
        return true;
    }
}