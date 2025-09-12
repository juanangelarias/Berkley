using HotChocolate.Authorization;
using James.Shared;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        /// <summary>
        /// Search agencies by AgencyNumber or full name
        /// </summary>
        /// <param name="stringToSearch">Search string</param>
        /// <param name="activeOnly">True to only return active results, false to return all statuses</param>
        /// <param name="contextFactory">database context</param>
        /// <returns>Matching agencies</returns>
        [Authorize]
        public async Task<List<Agency>> SearchAgencies(string? stringToSearch, bool activeOnly, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            if (!string.IsNullOrWhiteSpace(stringToSearch))
            {
                var likeString = $"%{stringToSearch}%";
                //NOTE:This assumes that AgencyNumbers are always digits only
                var byAgencyNum = stringToSearch.IsDigitsOnly() ? ctx.Agencies.Include(a => a.IdNavigation)
                    .Include(a => a.IdNavigation.LegalEntityAddresses)
                    .ThenInclude(a => a.Address)
                    .Include(a => a.IdNavigation.LegalEntityEmails)
                    .Where(a => EF.Functions.Like(a.AgencyNumber, likeString) && (activeOnly || a.Status=="Active")).ToListAsync() : Task.FromResult(new List<Agency>());
                var ctx2 = await contextFactory.CreateDbContextAsync();
                var byName = ctx2.Agencies.Include(a => a.IdNavigation)
                    .Include(a => a.IdNavigation.LegalEntityAddresses)
                    .ThenInclude(a => a.Address)
                    .Include(a => a.IdNavigation.LegalEntityEmails)
                    .Where(a => EF.Functions.Like(a.IdNavigation.FullName, likeString) && (activeOnly || a.Status=="Active")).ToListAsync();
                var results = await byAgencyNum;
                var byNameResults = await byName;
                results.AddRange(byNameResults);
                return results;
            }
            //Allow this for testing, but shouldn't be allowed by UI
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
                .ThenInclude(a => a!.AgencyIdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)
                .Include(a => a.AgencyErrorAndOmissions)

                .FirstOrDefaultAsync();
            return result ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");
        }

        /// <summary>
        /// Gets basic agency info such as Agency number and name from the agency's Id
        /// </summary>
        /// <param name="agencyId">Agency id</param>
        /// <param name="contextFactory">database entities context</param>
        /// <returns>Basic agency object with agency number, name and other first-level properties</returns>
        /// <remarks>For a more hydrated agency object, use GetAgencyByAgencyNumber</remarks>
        /// <exception cref="GraphQLException">No agency with the agencyId exists</exception>
        [Authorize]
        public async Task<Agency?> GetAgencyById(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Agencies.Where(a => a.Id == agencyId)
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync();
            
            return result ?? throw new GraphQLException($"No agency exists with Id {agencyId}.");
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
                throw new GraphQLException($"Error when retrieving status log for agency {agencyNumber}", ex);
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

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}.");
        }

        [Authorize]
        public async Task<List<Agent>> GetAgencyAgents(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AgentsInAgencies.Where(ag => ag.AgencyId == agencyId)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.IdNavigation)
                .ThenInclude(agi=>agi.LegalEntityEmails)
                .Include(ag => ag.Agent)
                .ThenInclude(ag => ag.AgencyLicenses)
                .ThenInclude(ag => ag.Insurer)
                .ThenInclude(ag => ag.IdNavigation)
                .Where(ag => ag.AgencyId == agencyId)
                .Select(aia => aia.Agent)
                .ToListAsync();
        }

        [Authorize]
        public async Task<List<AgencyDto>> GetAllActiveAgencies([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var agencies = await ctx.Agencies
                .Include(i => i.IdNavigation)
                .Include(i => i.IdNavigation)
                .ThenInclude(t => t.LegalEntityAddresses)
                .ThenInclude(t1 => t1.Address)
                .Where(a => a.Status == "Active")
                .Select(s => new AgencyDto
                {
                    Id = s.Id,
                    AgencyNumber = s.AgencyNumber,
                    FullName = $"({s.AgencyNumber}) {s.IdNavigation.FullName}",
                    Addresses = s.IdNavigation.LegalEntityAddresses
                        .Select(s1 => new AddressDto
                        {
                            Id = s1.AddressId,
                            Type = s1.Type,
                            Address1 = s1.Address.Address1,
                            Address2 = s1.Address.Address2 ?? "",
                            Address3 = s1.Address.Address3 ?? "",
                            City = s1.Address.City,
                            StateCode = s1.Address.StateCode ?? "",
                            PostalCode = s1.Address.PostalCode ?? "",
                            CountryCode = ""
                        })
                        .ToList(),
                    Emails = s.IdNavigation.LegalEntityEmails
                        .Select(s2=> new EmailDto
                        {
                            Id = s2.Id,
                            Type = s2.Type,
                            EmailAddress = s2.EmailAddress
                        })
                        .ToList()
                })
                .ToListAsync();

            return agencies;
        }

        [Authorize]
        public async Task<List<AgencyLicense>> GetAgencyLicenses(Guid agencyId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.AgencyLicenses.Where(lic => lic.AgencyId == agencyId && lic.AgentId == null)
                .Include(lic => lic.Agency)
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
                .Include(p => p.Agency)
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
                    .Include(a => a.AgencyLicenses)
                    .ThenInclude(al => al.Agent)
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

        [Authorize]
        public async Task<List<Agency>> GetIdAgencyNumbers([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var agencyList = await ctx.Agencies.ToListAsync();
            return agencyList;
        }
    }
}
