using HotChocolate.Types;
using James.Data.Server.Model;

namespace James.Data.Server.GraphQL.TypeExtensions;

[ExtendObjectType(typeof(Agent), IgnoreProperties = ["Accounts"],
    IgnoreFields = null)]
public class AgentExtensions
{
    public async Task<Account[]> Accounts([Parent] Agent agent,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        if (agent.Accounts.Count == 0)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            agent.Accounts = await ctx.Accounts.Where(a => a.AgentId == agent.Id).ToArrayAsync();
        }
        return agent.Accounts.ToArray();
    }
    public async Task<AgencyLicense[]> Licenses([Parent] Agent agent, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        if (agent.AgencyLicenses.Count == 0)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            agent.AgencyLicenses = await ctx.AgencyLicenses.Where(a => a.AgentId == agent.Id).ToArrayAsync();
        }
        return agent.AgencyLicenses.ToArray();
    }
}