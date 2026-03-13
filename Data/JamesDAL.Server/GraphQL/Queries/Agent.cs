using HotChocolate.Authorization;
using James.Shared.Dto;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Agent> GetAgentByAgentId(Guid agentId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
        public async Task<List<Agent>> SearchAgents(string searchString,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.Agents
                .Include(a => a.IdNavigation)
                .Where(a => a.IdNavigation.FullName.Contains(searchString))
                .ToListAsync();
            ;
        }

        [Authorize]
        public async Task<List<AgencyLicense>> GetAgentLicenses(Guid agentId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.AgencyLicenses.Where(a => a.AgentId == agentId).ToListAsync();
        }

        [Authorize]
        public async Task<AgencyAgentDto?> VerifyNpn(string nationalProducerNumber, Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            
            var agent = await ctx.Agents
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityEmails)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityPhones)
                .ThenInclude(a => a.PhoneNumber)
                .FirstOrDefaultAsync(a => a.NationalProducerNumber == nationalProducerNumber);
            
            if (agent == null)
                return null;

            return new()
            {
                Id = Guid.NewGuid(),
                AgentId = agent.Id,
                AgencyId = agencyId,
                NationalProducerNumber = agent.NationalProducerNumber,
                FullName = agent.IdNavigation.FullName,
                GivenName = agent.IdNavigation.GivenName,
                FamilyName = agent.IdNavigation.FamilyName,
                Email = agent.IdNavigation.LegalEntityEmails
                    .FirstOrDefault(f => f.Type == "Main")?
                    .EmailAddress ?? "",
                CountryCode = agent.IdNavigation.LegalEntityPhones
                    .FirstOrDefault(f => f.Type == "Main")?
                    .PhoneNumber?
                    .CountryCode ?? "",
                PhoneNumber = agent.IdNavigation.LegalEntityPhones
                    .FirstOrDefault(f => f.Type == "Main")?
                    .PhoneNumber?
                    .MainNumber ?? "",
                Extension = agent.IdNavigation.LegalEntityPhones
                    .FirstOrDefault(f => f.Type == "Main")?
                    .PhoneNumber?
                    .Extension ?? "",
                AIF = false
            };
        }
    }
}