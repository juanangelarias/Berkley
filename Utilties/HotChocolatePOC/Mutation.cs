using HotChocolate.Subscriptions;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace HotChocolatePOC
{
    [MutationType]
    public class BankMutationType : ObjectType<Mutation>
    {
    }

    public partial class Mutation
    {
        public async Task<Account> UpdateBank(UpdateBankInput input,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var account = ctx.Accounts.Include(a => a.IdNavigation).SingleOrDefault(a => a.AccountNum == input.AccountNum);
            if (account == null)
                throw new GraphQLException("Invalid account number");
            if (input.Bank != account.Bank)
            {
                account.Bank = input.Bank;
                //TODO:  Figure out if only 1 of the 2 below lines are needed.
                ctx.Update(account);
                await ctx.SaveChangesAsync();
                await eventSender.SendAsync(nameof(Mutation.UpdateBank), account);
            }
            return account;
        }
    }

    public class UpdateBankInput
    {
        public string AccountNum { get; set; }
        public string Bank { get; set; }
    }
}
