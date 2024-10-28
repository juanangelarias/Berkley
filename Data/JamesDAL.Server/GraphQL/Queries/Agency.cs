using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
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

        [Authorize]
        public async Task<Agency?> GetAgencyByAgencyNumber(string agencyNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Agencies.Where(a => a.AgencyNumber == agencyNumber)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.ParentNavigation)
                .ThenInclude(a => a.AgencyIdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)
                .Include(a => a.AgencyErrorAndOmissions)

                .FirstOrDefaultAsync();
            return result ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");
        }

        [Authorize]
        public async Task<List<Account>> GetAgencyAccounts(string agencyNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Accounts.Where(a => a.AgencyNumber == agencyNumber)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityAddresses.Where(lea => lea.Type == "Main"))
                .ThenInclude(a => a.Address)
                .Include(a => a.Bonds)
                .ThenInclude(a => a.UnderWriter)
                .ThenInclude(a => a.IdNavigation)
                .Include(a => a.Bonds)
                .ThenInclude(a => a.Obligee)
                .ToListAsync();
            return result ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");

        }

        [Authorize]
        public async Task<Agency> GetAgencyParent(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Agencies.Where(a => a.Id == agencyId)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.ParentNavigation)
                .FirstOrDefaultAsync();

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}.");
        }
        [Authorize]
        public async Task<List<AgencyInventory>> GetAgencyInventory(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.AgencyInventories.Where(a => a.AgencyId == agencyId)
                .Include(a => a.Address)
                .ToListAsync();

            return result ?? throw new GraphQLException($"No agency inventory exists with agencyId {agencyId}.");
        }
        [Authorize]
        public async Task<List<AgencyStatusLog>> GetAgencyStatusLog(string agencyNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var result = await ctx.AgencyStatusLogs.Where(a => a.AgencyNumber == agencyNumber)
                    .Include(a => a.ChangedByNavigation)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Error when retreiving status log for agency {agencyNumber}", ex);
            }

        }
        [Authorize]
        public async Task<List<Bond>> GetAgencyBonds(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Bonds.Where(a => a.AgencyId == agencyId)
                .Include(b => b.UnderWriter)
                .ThenInclude(b => b.IdNavigation)
                .Include(b => b.Obligee)
                .Include(b => b.AccountNumNavigation)
                .ThenInclude(b => b.IdNavigation)
                .Include(b => b.BondType)
                .ToListAsync();

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}."); ;
        }

        [Authorize]
        public async Task<List<AgentsInAgency>> GetAgencyAgents(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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

        [Authorize]
        public async Task<List<AgencyLicense>> GetAgencyLicenses(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = await ctx.AgencyLicenses.Where(lic => lic.AgencyId == agencyId && lic.AgentId == null)
                .Include(lic=>lic.Agency)
                .Include(lic => lic.Insurer)
                .ThenInclude(lic => lic.IdNavigation)
                .ToListAsync();

            return result;
        }

        [Authorize]
        public async Task<List<AgencyStatusDm>> GetAgencyStatuses([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AgencyStatusDms.ToListAsync();
        }

        [Authorize]
        public async Task<List<PowerOfAttorney>> GetAgencyPOAs(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.PowerOfAttorneys
                .Where(p => p.AgencyId.Equals(agencyId))
                .Include(p => p.Insurer)
                .ThenInclude(i => i.IdNavigation)
                .Include(p => p.PowerOfAttorneyDocumentStatuses)
                .ThenInclude(p => p.DocumentType)
                .Include(p => p.StatusNavigation)
                .Include(p=>p.Agency)
                .ToListAsync();
        }

        [Authorize]
        public async Task<List<PowerOfAttorneyDocumentNameDm>> GetPOADocumentNames([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.PowerOfAttorneyDocumentNameDms.ToListAsync();
        }

        [Authorize]
        public async Task<List<PowerOfAttorneyStatusDm>> GetAllPoaStatuses([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.PowerOfAttorneyStatusDms.ToListAsync();
        }

        [Authorize]
        public async Task<List<Agency>> GetAgencyRelatedParties(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var topParent = await ctx.VAgencyParents.FirstOrDefaultAsync(a => a.Id == agencyId);

            if (topParent != null)
            {
                var relatedPartyIds = await ctx.VAgencyParents.Where(a => a.Parent == topParent.Parent).Select(a => a.Id).ToListAsync();
                var relatedAgencies = await ctx.Agencies.Where(a => relatedPartyIds.Contains(a.Id))
                    .Include(a => a.IdNavigation)
                    .ThenInclude(a => a.LegalEntityAddresses.Where(lea => lea.Type == "Main"))
                    .ThenInclude(a => a.Address)
                    .ToListAsync();
                return relatedAgencies;
            }
            else
            {
                return new List<Agency>();
            }
        }

        [Authorize]
        public async Task<List<AgencyCommission>> GetAgencyCommissionRates(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AgencyCommissions.Where(ac => ac.AgencyId == agencyId).ToListAsync();
        }

        //[Authorize]
        //public async Task<List<>>
    }
}
