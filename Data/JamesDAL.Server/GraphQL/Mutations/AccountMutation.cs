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
    }
}
