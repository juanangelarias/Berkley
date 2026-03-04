using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Agent> GetAgentByAgentId(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            var agent = await ctx.Agents
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityEmails)
                .Include(a => a.AgencyLicenses)
                .ThenInclude(a => a.Insurer.IdNavigation)
                .Where(a => a.Id == agentId).FirstOrDefaultAsync();

            return agent ?? throw new Exception("Agent Id not found");
        }
        [Authorize]
        public async Task<List<Agent>> SearchAgents(string searchString, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.Agents
                .Include(a => a.IdNavigation)
                .Where(a => a.IdNavigation.FullName.Contains(searchString))
                .ToListAsync(); ;
        }
        [Authorize]
        public async Task<List<AgencyLicense>> GetAgentLicenses(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.AgencyLicenses.Where(a => a.AgentId == agentId).ToListAsync();
        }
    }
}