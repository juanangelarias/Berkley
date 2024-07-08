using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Agent> GetAgentByAgentId(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.Agents
                .Include(a => a.IdNavigation)
                .Include(a => a.AgencyLicenses)
                .Where(a => a.Id == agentId).FirstOrDefaultAsync();
        }

        [Authorize]
        public async Task<List<AgencyLicense>> GetAgentLicenses(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.AgencyLicenses.Where(a => a.AgentId == agentId).ToListAsync();
        }
    }
}