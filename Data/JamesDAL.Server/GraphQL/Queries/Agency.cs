using HotChocolate;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        //UNDONE: Change these all to sync Tasks
        public async Task<List<Agency>> SearchAgencies(string? stringToSearch, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            if (!string.IsNullOrWhiteSpace(stringToSearch))
                return await ctx.Agencies.Include(a => a.IdNavigation)
                    .Include(a => a.IdNavigation.LegalEntityAddresses)
                    .ThenInclude(a => a.Address)
                    .Where(a => a.IdNavigation.FullName.ToLower().Contains(stringToSearch.ToLower()))
                    .ToListAsync();
            else
                return await ctx.Agencies.Include(a => a.IdNavigation)
                    .ToListAsync();
        }
        public Agency? GetAgencyByAgencyNumber(string agencyNumber, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = ctx.Agencies.Where(a => a.AgencyNumber == agencyNumber)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.ParentNavigation)
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
        public async Task<List<Bond>> GetAgencyBonds(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Bonds.Where(a => a.AgencyId == agencyId)
                .Include(b => b.UnderWriter)
                .Include(b => b.Obligee)
                .ToListAsync(); 

            return result;
        }
        public async Task<List<AgentsInAgency>> GetAgencyAgents(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.AgentsInAgencies.Where(ag => ag.AgencyId == agencyId)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.IdNavigation)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.AgencyLicenses)
                .ThenInclude(ag => ag.Insurer)
                .ThenInclude(ag => ag.IdNavigation)
                .Where(ag => ag.AgencyId == agencyId)
                .ToListAsync();
        }

        public async Task<List<AgencyLicense>> GetAgencyLicenses(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = await ctx.AgencyLicenses.Where(lic => lic.AgencyId == agencyId && lic.AgentId == null)
                .Include(lic => lic.Insurer)
                .ThenInclude(lic => lic.IdNavigation)
                .ToListAsync();

            return result;
        }

        public async Task<List<AgencyStatusDm>> GetAgencyStatuses([Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AgencyStatusDms.ToListAsync();
        }

        public async Task<List<PowerOfAttorney>> GetAgencyPOAs(Guid agencyId, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.PowerOfAttorneys
                .Where(p => p.AgencyId.Equals(agencyId))
                .Include(p=>p.Insurer)
                .ThenInclude(i => i.IdNavigation)
                .Include(p => p.PowerOfAttorneyDocumentStatuses)
                .ThenInclude(p => p.DocumentType)
                .Include(p => p.StatusNavigation)
                .ToListAsync();
        }
    }
}
