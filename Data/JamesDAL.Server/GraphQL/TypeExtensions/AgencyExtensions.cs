using HotChocolate.Types;
using James.Data.Server.Model;

namespace James.Data.Server.GraphQL.TypeExtensions
{
    //TODO:Figure out how to unit test
    [ExtendObjectType(typeof(Agency), IgnoreProperties = new[] { "BillingContact", "OnlineBondSystems" }, IgnoreFields = null)]
    public class AgencyExtensions
    {
        public async Task<LegalEntity?> BillingContact([Parent] Agency agency,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (agency.BillingContact == null)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                agency.BillingContact =
                    await ctx.LegalEntities.SingleOrDefaultAsync(le => le.Id == agency.BillingContactId);
            }
            return agency.BillingContact;
        }

        public async Task<Agent[]> Agents([Parent] Agency agency,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == agency.AgentsInAgencies.FirstOrDefault()?.Agent)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                if (!agency.AgentsInAgencies.Any())
                    agency.AgentsInAgencies = await ctx.AgentsInAgencies
                        .Where(aia => aia.AgencyId == agency.Id)
                        .Include(aia => aia.Agent).ToArrayAsync();
                foreach (var aia in agency.AgentsInAgencies)
                    //Hydrate if needed
                    aia.Agent ??= (ctx.Agents.Single(a => a.Id == aia.AgentId));
            }
            return agency.AgentsInAgencies.Select(aia => aia.Agent).ToArray();
        }

        public async Task<Account[]> Accounts([Parent] Agency agency, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == agency.Accounts)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                if (!agency.Accounts.Any())
                {
                    agency.Accounts = await ctx.Accounts.Where(ac => ac.AgencyNumber == agency.AgencyNumber).ToArrayAsync();
                    
                }
            }
            return agency.Accounts.ToArray();
        }
        public async Task<OnlineBondSystem[]> OnlineBondSystems([Parent] Agency agency,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (agency.OnlineBondSystems.Count==0)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                agency.OnlineBondSystems = await ctx.OnlineBondSystems.Where(obs=>obs.LegalEntityId== agency.Id).ToArrayAsync();
            }
            return agency.OnlineBondSystems.ToArray();
        }

        public async Task<AgencyInventory[]> Inventories([Parent] Agency agency,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (agency.AgencyInventories.Count == 0)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                agency.AgencyInventories = await ctx.AgencyInventories.Where(ai => ai.AgencyId == agency.Id).ToArrayAsync();
            }
            return agency.AgencyInventories.ToArray();
        }

        public async Task<AgencyErrorAndOmission[]> ErrorAndOmissions([Parent] Agency agency,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (agency.AgencyErrorAndOmissions.Count == 0)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                agency.AgencyErrorAndOmissions = await ctx.AgencyErrorAndOmissions.Where(aoe => aoe.AgencyId == agency.Id).ToArrayAsync();
            }
            return agency.AgencyErrorAndOmissions.ToArray();
        }

        public async Task<AgencyCompetition[]> Competition([Parent] Agency agency,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (agency.AgencyCompetitions.Count == 0)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                agency.AgencyCompetitions = await ctx.AgencyCompetitions.Where(ac => ac.AgencyId == agency.Id).ToArrayAsync();
            }
            return agency.AgencyCompetitions.ToArray();
        }
    }
}
