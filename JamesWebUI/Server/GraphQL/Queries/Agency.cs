using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.Queries
{
    public partial class Query
    {
        public List<Agency> SearchAgencies(string? stringToSearch, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            if (!string.IsNullOrWhiteSpace(stringToSearch))
                return ctx.Agencies.Include(a => a.IdNavigation)
                    .Include(a => a.IdNavigation.LegalEntityAddresses)
                    .ThenInclude(a => a.Address)
                    .Where(a => a.IdNavigation.FullName.ToLower().Contains(stringToSearch.ToLower()))
                    .ToList();
            else
                return ctx.Agencies.Include(a => a.IdNavigation)
                    .ToList();
        }
        public Agency? GetAgencyByAgencyNumber(string agencyNumber, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = ctx.Agencies.Where(a => a.AgencyNumber == agencyNumber)
                .Include(a => a.IdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)
                .Include(a => a.AgencyInventories)
                .Include(a => a.Accounts)
                .Include(a => a.AgentsInAgencies)
                .FirstOrDefault();
            return result ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");
        }
        public List<AgentsInAgency> GetAgencyAgents(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.AgentsInAgencies.Where(ag => ag.AgencyId == agencyId)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.IdNavigation)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.AgencyLicenses)
                .ToList();
        }
        public List<Bond> GetAgencyBonds(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.Bonds.Where(b => b.AgencyId == agencyId).ToList();
        }
        public Task<List<AgencyLicense>>? GetAgencyLicenses(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result =  ctx.AgencyLicenses.Where(lic => lic.AgencyId == agencyId && lic.AgentId == null)
                .Include(lic => lic.Insurer)
                .ThenInclude(lic => lic.IdNavigation)
                .ToListAsync();

            return result;
        }

        public List<AgencyStatusDm> GetAgencyStatuses([Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            return ctx.AgencyStatusDms.ToList();
        }
    }
}
