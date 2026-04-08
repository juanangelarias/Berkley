using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<AgentSystemDm>> GetOnlineSystems(
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.AgentSystemDms
            .OrderBy(o=>o.SystemName)
            .ToListAsync();
    }
    
    [Authorize]
    public async Task<List<OnlineBondSystem>> GetOnlineBondSystemsByLegalEntity(Guid legalEntityId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.OnlineBondSystems
            .Include(i=>i.Insurer)
            .ThenInclude(i=>i.IdNavigation)
            .Where(o => o.LegalEntityId == legalEntityId)
            .OrderBy(o => o.SystemName)
            .ToListAsync();
    }
}