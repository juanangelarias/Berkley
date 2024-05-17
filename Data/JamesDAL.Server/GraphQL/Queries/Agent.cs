using HotChocolate;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    { 
        public Agent GetAgentByAgentId(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.Agents
                .Include(a => a.IdNavigation)
                .Include(a => a.AgencyLicenses)
                .Where(a => a.Id == agentId).FirstOrDefault();
        }
        public List<AgencyLicense> GetAgentLicenses(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.AgencyLicenses.Where(a => a.AgentId == agentId).ToList();

        }
    }
}