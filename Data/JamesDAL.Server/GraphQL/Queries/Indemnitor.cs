using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<Indemnitor>> GetIndemnitorsByAccount(string accountNum, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var indemnitors = await ctx.Indemnitors
            .Include(i=>i.IdNavigation)
            .Where(i => i.AccountNum == accountNum)
            .ToListAsync();

        return indemnitors;
    }
}