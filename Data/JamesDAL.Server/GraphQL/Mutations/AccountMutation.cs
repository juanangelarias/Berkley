using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class AccountMutation
    {
        [Authorize]
        public async Task<bool> SetCurrentCreditReportLink(string accountNum, Guid? imagingDocumentId,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var acct = ctx.Accounts.First(p => p.AccountNum == accountNum);
                acct.CreditReportImagingId = imagingDocumentId;
                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> SetAccountGeneralInfo(Guid accountId, string? yearStarted, string? currentManagementYear, string? businessClass,
            string? businessType, string? priorSurety, int? estAnnualPremium, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            try
            {
                var account = await ctx.Accounts
                    .Include(a => a.IdNavigation)
                    .FirstOrDefaultAsync(a => a.Id == accountId);

                account.YearOpened = yearStarted;
                account.CurrentManagementYear = currentManagementYear;
                account.BusinessTypeClass = businessClass;
                account.BusinessType = businessType;
                account.PriorSuretyCompany = priorSurety;

                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

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

