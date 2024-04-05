using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.Queries
{
    public partial class Query
    { 
        public List<AgencyLicense> GetAgentLicenses(Guid agentId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.AgencyLicenses.Where(a => a.AgentId == agentId).ToList();

        }
    }
}