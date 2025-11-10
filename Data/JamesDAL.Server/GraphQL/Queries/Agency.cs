using HotChocolate.Authorization;
using James.Shared;
using James.Shared.Dto;

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
        public async Task<List<AgencySearchDto>> SearchAgencies(string? stringToSearch, bool activeOnly,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            if (string.IsNullOrWhiteSpace(stringToSearch))
                return [];

            var likeString = $"%{stringToSearch}%";
            var filter = stringToSearch.ToLower().Trim();

            var agencies = await ctx.Agencies.Include(a => a.IdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)
                .Include(a => a.IdNavigation.LegalEntityEmails)
                .Where(a => !activeOnly || a.Status == "Active")
                .Select(s => new AgencySearchDto
                {
                    Id = s.Id,
                    AgencyNumber = s.AgencyNumber,
                    AgencyName = s.IdNavigation.FullName,
                    City = s.IdNavigation.LegalEntityAddresses
                        .FirstOrDefault(f => f.Type == "Main")!
                        .Address.City + ", " + s.IdNavigation.LegalEntityAddresses
                        .FirstOrDefault(f => f.Type == "Main")!.Address.StateCode,
                    Branch = s.Branch,
                    Status = s.Status,
                    ParentChild = s.Id == s.IdNavigation.Parent ? "P" : "C"
                })
                .ToListAsync();

            var results = agencies
                .Where(a => a.AgencyNumber.ToLower().Contains(filter) ||
                            a.AgencyName.ToLower().Contains(filter) ||
                            a.City.ToLower().Contains(filter) ||
                            a.Branch.ToLower().Contains(filter))
                .ToList();

            return results;
        }

        [Authorize]
        public async Task<Agency?> GetAgencyByAgencyNumber(string agencyNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
        public async Task<Agency?> GetAgencyById(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Agencies.Where(a => a.Id == agencyId)
                .Include(a => a.IdNavigation)
                .FirstOrDefaultAsync();
            return result ?? throw new GraphQLException($"No agency exists with Id {agencyId}.");
        }

        /// <summary>
        /// Retrieves a list of agency accounts based on the provided agency number.
        /// </summary>
        /// <param name="agencyNumber">The unique Agency Number for the agency to fetch accounts for.</param>
        /// <param name="contextFactory">The database context factory to access the data store.</param>
        /// <returns>A list of agency accounts matching the given agency number.</returns>
        [Authorize]
        public async Task<List<AgencyAccountDto>> GetAgencyAccounts(string agencyNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var view = await ctx.VAccounts
                .Where(a => a.AgencyNumber == agencyNumber)
                .ToListAsync();

            var response = view.Select(account => new AgencyAccountDto
                {
                    Status = account.AccountStatus,
                    AccountNum = account.AccountNum,
                    Name = account.FullName,
                    Branch = account.Branch,
                    BranchFullName = account.BranchName,
                    MainAddress = new Address
                    {
                        Id = Guid.NewGuid(),
                        Address1 = account.Address1 ?? "",
                        Address2 = account.Address2,
                        Address3 = account.Address3,
                        City = account.City ?? "",
                        StateCode = account.StateCode,
                        PostalCode = account.PostalCode,
                    },
                    Bonds = []
                })
                .ToList();

            return response ?? throw new GraphQLException($"No agency exists with agencyNumber {agencyNumber}.");
        }

        [Authorize]
        public async Task<List<AgencyAccountBondDto>> GetAgencyAccountBonds(string accountNum,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var bonds = await ctx.Bonds
                .Include(i => i.UnderWriter)
                .ThenInclude(t => t.IdNavigation)
                .Include(i => i.Obligee)
                .Include(i => i.BondType)
                .Include(i => i.BondTransactions)
                .Where(r => r.AccountNum == accountNum)
                .ToListAsync();

            var response = bonds
                .Select(r => new AgencyAccountBondDto
                {
                    BondNumber = r.BondNumber,
                    Amount = r.BondTransactions
                        .OrderByDescending(o => o.BondMod)
                        .ThenByDescending(t => t.GroupNumber)
                        .FirstOrDefault()?
                        .BondAmount ?? 0,
                    Effective = r.Effective,
                    Expiration = r.Expiration,
                    UnderwriterId = r.UnderWriterId,
                    UnderwriterFullName = r.UnderWriter.IdNavigation.FullName,
                    ObligeeId = r.ObligeeId,
                    ObligeeFullName = r.Obligee?.FullName,
                    BondType = r.BondType?.BondType,
                    Siccode = r.Siccode,
                    Municipality = r.Municipality,
                    BondClass = r.BondClass,
                    Status = r.Status,
                    Appointment = r.BondTransactions.FirstOrDefault(f => f.Type == "Initial Premium")?
                        .BillDate ?? DateTime.MinValue,
                    Termination = r.BondTransactions.FirstOrDefault(f => f.Type == "Closing")?
                        .BillDate ?? DateTime.MaxValue,
                })
                .ToList();

            return response ?? throw new GraphQLException($"No Account exists with accountNum {accountNum}.");
        }

        [Authorize]
        public async Task<Agency> GetAgencyParent(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Agencies.Where(a => a.Id == agencyId)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.ParentNavigation)
                .FirstOrDefaultAsync();

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}.");
        }

        [Authorize]
        public async Task<List<AgencyInventory>> GetAgencyInventory(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.AgencyInventories.Where(a => a.AgencyId == agencyId)
                .Include(a => a.Address)
                .ToListAsync();

            return result ?? throw new GraphQLException($"No agency inventory exists with agencyId {agencyId}.");
        }

        [Authorize]
        public async Task<List<AgencyStatusLog>> GetAgencyStatusLog(string agencyNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
        public async Task<List<Bond>> GetAgencyBonds(Guid agencyId, int skip, int take,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Bonds.Where(a => a.AgencyId == agencyId)
                .Include(b => b.UnderWriter)
                .ThenInclude(b => b.IdNavigation)
                .Include(b => b.Obligee)
                .Include(b => b.AccountNumNavigation)
                .ThenInclude(i => i.AccountStatusLogs)
                .Include(i => i.AccountNumNavigation.IdNavigation)
                .Include(b => b.BondType)
                .OrderBy(o => o.AgencyId)
                .ThenBy(t => t.Effective)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return result ?? throw new GraphQLException($"No agency exists with agencyId {agencyId}.");
        }

        [Authorize]
        public async Task<QueryCount> GetAgencyBondsCount(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Bonds.CountAsync(a => a.AgencyId == agencyId);
            
            return new QueryCount{Count = result};
        }

        [Authorize]
        public async Task<List<Agent>> GetAgencyAgents(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
                .Select(aia => aia.Agent)
                .ToListAsync();
        }

        [Authorize]
        public async Task<List<AgencyLicense>> GetAgencyLicenses(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            var result = await ctx.AgencyLicenses.Where(lic => lic.AgencyId == agencyId && lic.AgentId == null)
                .Include(lic => lic.Agency)
                .Include(lic => lic.Insurer)
                .ThenInclude(lic => lic.IdNavigation)
                .ToListAsync();

            return result;
        }

        [Authorize]
        public async Task<List<AgencyStatusDm>> GetAgencyStatuses(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AgencyStatusDms.ToListAsync();
        }

        [Authorize]
        public async Task<List<PowerOfAttorney>> GetAgencyPOAs(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
        public async Task<List<PowerOfAttorneyDocumentNameDm>> GetPOADocumentNames(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.PowerOfAttorneyDocumentNameDms.ToListAsync();
        }

        [Authorize]
        public async Task<List<PowerOfAttorneyStatusDm>> GetAllPoaStatuses(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.PowerOfAttorneyStatusDms.ToListAsync();
        }

        [Authorize]
        public async Task<List<AgencyLocationsDto>> GetAgencyRelatedParties(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var topParent = await ctx.VAgencyParents.FirstOrDefaultAsync(a => a.Id == agencyId);

            if (topParent == null)
                return [];

            var relatedPartyIds = await ctx.VAgencyParents.Where(a => a.Parent == topParent.Parent).Select(a => a.Id)
                .ToListAsync();

            var relatedAgencies = await ctx.Agencies.Where(a => relatedPartyIds.Contains(a.Id))
                .Include(a => a.AgencyLicenses)
                .ThenInclude(al => al.Agent)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityAddresses.Where(lea => lea.Type == "Main"))
                .ThenInclude(a => a.Address)
                .ToListAsync();

            var result = relatedAgencies
                .Select(s => new AgencyLocationsDto
                {
                    IsTopParent = s.Id == topParent.Parent,
                    Agency = s
                })
                .ToList();

            return result.OrderByDescending(o => o.IsTopParent).ThenBy(t => t.Agency.IdNavigation.FamilyName).ToList();
        }

        [Authorize]
        public async Task<List<AgencyCommission>> GetAgencyCommissionRates(Guid agencyId,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AgencyCommissions.Where(ac => ac.AgencyId == agencyId).ToListAsync();
        }

        [Authorize]
        public async Task<List<Agency>> GetIdAgencyNumbers(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var agencyList = await ctx.Agencies.ToListAsync();
            return agencyList;
        }
    }
}