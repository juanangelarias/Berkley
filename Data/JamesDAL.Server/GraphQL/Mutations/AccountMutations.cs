using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Collections.Immutable;
using System.Diagnostics;
using James.Shared;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class AccountMutations
    {
        [Authorize]
        public async Task<AccountProgram> SetAccountProgram(Guid programId, DateTime effective, DateTime expritation, int single, int aggregate,
            string? comments, Guid statusId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            //TODO: Implement. Need to deal with status changes via business logic
            var ctx = await contextFactory.CreateDbContextAsync();

            var oldProgram = await ctx.AccountPrograms.FirstOrDefaultAsync(a => a.Id == programId);
            
            //Update Status history before we set the new values for the program
            //TODO: Add StatusChangeBy
            AccountProgramStatusHistory newHistory = new AccountProgramStatusHistory
            {
                OldSingle = oldProgram.Single,
                NewSingle = single,
                OldAggregate = oldProgram.Aggregate,
                NewAggregate = aggregate,
                OldStatus = oldProgram.StatusId,
                NewStatus = statusId,
                StatusDate = DateTime.Now,
                AccountProgramId = programId
            };

            oldProgram.Effective = effective;
            oldProgram.Expiration = expritation;
            oldProgram.Single = single;
            oldProgram.Aggregate = aggregate;
            oldProgram.Comments = comments;
            oldProgram.StatusId = statusId;



            ctx.Update(oldProgram);
            await ctx.SaveChangesAsync();

            return oldProgram;
        }
    }

}
