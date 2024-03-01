using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.TypeExtensions
{
    [ExtendObjectType(typeof(Account))]
    public class AccountExtensions
    {
        public async Task<Agency?> GetAgency([Parent] Account account,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == account.AgencyNumberNavigation)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                account.AgencyNumberNavigation =
                    await ctx.Agencies.SingleOrDefaultAsync(agc => agc.AgencyNumber == account.AgencyNumber);
            }
            return account.AgencyNumberNavigation;
        }
    }
}
