using Microsoft.AspNetCore.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    public async Task<List<Underwriter>> GetUnderwriters(
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.Underwriters
            .Include(i=>i.IdNavigation)
            .ToListAsync();
    }

    [Authorize]
    public async Task<List<UnderwriterRecommendation>> GetUnderwriterRecommendationByAccount(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var result =  await ctx.UnderwriterRecommendations
            .Include(i=>i.PostedByNavigation)
            .Where(r => r.AccountNum == accountNumber)
            .OrderBy(o=>o.AccountNum)
            .ThenByDescending(o=>o.Created)
            .ToListAsync();
        
        return result;
    }
}