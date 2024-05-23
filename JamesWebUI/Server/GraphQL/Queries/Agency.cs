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
                .ThenInclude(a => a.ParentNavigation)
                .ThenInclude(a => a.AgencyIdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)

                .FirstOrDefault();
            return result ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");
        }
        public List<Account> GetAgencyAccounts(string agencyNumber, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = ctx.Accounts.Where(a => a.AgencyNumber == agencyNumber)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityAddresses.Where(lea => lea.Type == "Main"))
                .ThenInclude(a => a.Address)
                .Include(a => a.Bonds)
                .ThenInclude(a => a.UnderWriter)
                .ThenInclude(a => a.IdNavigation)
                .Include(a => a.Bonds)
                .ThenInclude(a => a.Obligee)
                .ToList();
            return result ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");
                
        }
        public Agency GetAgencyParent(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = ctx.Agencies.Where(a => a.Id == agencyId)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.ParentNavigation)
                .FirstOrDefault();

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}.");
        }
        public List<Bond> GetAgencyBonds(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = ctx.Bonds.Where(a => a.AgencyId == agencyId)
                .Include(b => b.UnderWriter)
                .ThenInclude(b => b.IdNavigation)
                .Include(b => b.Obligee)
                .ToList();

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}."); ;
        }
        public List<AgentsInAgency> GetAgencyAgents(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.AgentsInAgencies.Where(ag => ag.AgencyId == agencyId)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.IdNavigation)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.AgencyLicenses)
                .ThenInclude(ag => ag.Insurer)
                .ThenInclude(ag => ag.IdNavigation)
                .Where(ag => ag.AgencyId == agencyId)
                .ToList();
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

        public List<PowerOfAttorney> GetAgencyPOAs(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            return ctx.PowerOfAttorneys
                .Where(p => p.AgencyId.Equals(agencyId))
                .Include(p=>p.Insurer)
                .ThenInclude(i => i.IdNavigation)
                .Include(p => p.PowerOfAttorneyDocumentStatuses)
                .ThenInclude(p => p.DocumentType)
                .Include(p => p.StatusNavigation)
                .ToList();
        }

        public List<PowerOfAttorneyDocumentNameDm> GetPOADocumentNames([Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            return ctx.PowerOfAttorneyDocumentNameDms.ToList();
        }
        public List<PowerOfAttorneyStatusDm> GetAllPoaStatuses([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            return ctx.PowerOfAttorneyStatusDms.ToList();
        }
    }
}
