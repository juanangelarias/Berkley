using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Account?> GetAccountByNumber(string? accountNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (accountNumber == null)
                return null;
            var ctx = await contextFactory.CreateDbContextAsync();
            var data = await ctx.Accounts
                           // Legal Entity
                           .Include(a => a.IdNavigation)
                           // Addresses
                           .Include(a => a.IdNavigation.LegalEntityAddresses)
                           .ThenInclude(a => a.Address)
                           // Phones
                           .Include(a => a.IdNavigation.LegalEntityPhones)
                           .ThenInclude(a => a.PhoneNumber)
                           // Emails
                           .Include(a => a.IdNavigation.LegalEntityEmails)
                           // Agency
                           .Include(a => a.AgencyNumberNavigation)
                           // Agency Legal Entity
                           .ThenInclude(ag => ag!.IdNavigation)
                           // Agency Addresses
                           .ThenInclude(agi => agi.LegalEntityAddresses)
                           .ThenInclude(agia => agia.Address)
                           // Underwriter
                           .Include(a => a.Underwriter)
                           // Underwriter Employee
                           .ThenInclude(uw => uw!.IdNavigation)
                           // Agent
                           .Include(a => a.Agent)
                           .ThenInclude(ag => ag!.IdNavigation)
                           // Home Office Review By
                           .Include(a => a.HomeOfficeReviewByNavigation)
                           // Branch Review By
                           .Include(a => a.BranchReviewByNavigation)
                           // Bank Phone
                           .Include(a => a.BankPhone)
                           // CPA firm (Legal Entity)
                           .Include(a => a.Cpafirm)
                           // CPA firm Phones
                           .ThenInclude(c => c!.LegalEntityPhones)
                           // CPA Contact (Legal Entity)
                           .Include(a => a.Cpacontact)
                           // Business Type
                           .Include(a => a.BusinessTypeNavigation)
                           // Business Type Class
                           .Include(a => a.BusinessTypeClassNavigation)
                           // Law Firm (Law Entity)
                           .Include(a => a.LawFirm)
                           // Law Firm (Legal Entity)
                           .ThenInclude(a => a!.IdNavigation)
                           // Account Statuses
                           .Include(i => i.AccountStatusLogs)
                           .ThenInclude(i => i.AccountStatusNavigation)
                           .FirstOrDefaultAsync(a => a.AccountNum.Trim() == accountNumber.Trim())
                       ?? throw new GraphQLException("No account with this account number exists.");

            return data;
        }

        [Authorize]
        public async Task<DateOnly?> GetFirstIndemnity(string accountNum,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var firstIndemnity = ctx.Indemnitors
                    .OrderBy(o => o.AgreementDate)
                    .FirstOrDefault(r => r.AccountNum == accountNum)?
                    .AgreementDate;

            return firstIndemnity;
        }
        
        [Authorize]
        public async Task<List<Account>> SearchAccounts(string searchString,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                //TODO: Improve search with fuzzy logic.
                var ctx = await contextFactory.CreateDbContextAsync();
                return await ctx.Accounts
                    .Where(a => a.IdNavigation.FullName.Contains(searchString) || a.AccountNum.Contains(searchString))
                    .Include(a => a.IdNavigation)
                    .ToListAsync();
            }
            else
            {
                return [];
            }
        }

        [Authorize]
        public async Task<InforceAccountLOA> GetAccountActiveLinesOfAuthority(string accountNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            var contractLOA = await ctx.LineOfAuthorityLogs
                .Where(l => l.AccountNum == accountNumber && l.Effective <= DateTime.Today && l.BondType == "Contract")
                .OrderByDescending(l => l.Created)
                .FirstOrDefaultAsync();

            var commercialLOA = await ctx.LineOfAuthorityLogs
                .Where(l => l.AccountNum == accountNumber && l.Effective <= DateTime.Today &&
                            l.BondType == "Commercial")
                .OrderByDescending(l => l.Created)
                .FirstOrDefaultAsync();

            InforceAccountLOA inforceLOAs = new InforceAccountLOA()
            {
                AccountNum = accountNumber,
                ContractLOA = contractLOA,
                CommercialLOA = commercialLOA
            };


            return inforceLOAs;
        }

        [Authorize]
        public async Task<List<AccountProgram>> GetAccountProgramHistory(string accountNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.AccountPrograms
                .Where(a => a.AccountNum == accountNumber)
                .Include(a => a.AccountProgramStatusHistories)
                .Include(a => a.Status)
                .OrderByDescending(a => a.Expiration)
                .ToListAsync();
        }

        [Authorize]
        public async Task<Account?> GetAccountOnly(string? accountNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (accountNumber == null)
                return null;
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Accounts
                .FirstOrDefaultAsync(a => a.AccountNum.Trim() == accountNumber.Trim());
        }

        [Authorize]
        public async Task<List<AdditionalRelatedParty>> GetAdditionalRelatedParties(string? accountNumber,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return null!;
            }

            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.AdditionalRelatedParties
                .Include(a => a.IdNavigation)
                .Where(a => a.AccountNum == accountNumber)
                .ToListAsync();
        }

        [Authorize]
        public async Task<List<Account>> GetIdAccountNumbers(
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var acctList = await ctx.Accounts.ToListAsync();
            return acctList;
        }

        [Authorize]
        public async Task<List<Account>> SearchAccountsByAccountNumber(string accountNumberFragment, bool activeOnly,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var matchingAccounts = await ctx.Accounts
                .Include(a => a.IdNavigation)
                .ThenInclude(le => le.LegalEntityAddresses)
                .ThenInclude(lea => lea.Address)
                .ThenInclude(ad => ad.StateCodeNavigation)
                .ThenInclude(sc => sc!.CountryCodeNavigation)
                .Include(a => a.IdNavigation.LegalEntityPhones)
                .ThenInclude(lep => lep.PhoneNumber)
                .Include(a => a.IdNavigation.LegalEntityEmails)
                .Join(ctx.VAccountStatuses, act => act.AccountNum, vact => vact.AccountNum,
                    (act, vact) => new { Account = act, Active = vact.AccountStatus == "Active" })
                .Where(a => EF.Functions.Like(a.Account.AccountNum, $"%{accountNumberFragment}%") &&
                            (activeOnly == false || a.Active))
                .Select(a => a.Account)
                .ToListAsync();
            return matchingAccounts;
        }

        [Authorize]
        public async Task<List<Account>> SearchAccountsByName(string searchString, bool activeOnly,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var matchingAccounts = await ctx.Accounts
                .Include(a => a.IdNavigation)
                .ThenInclude(le => le.LegalEntityAddresses)
                .ThenInclude(lea => lea.Address)
                .ThenInclude(ad => ad.StateCodeNavigation)
                .ThenInclude(sc => sc!.CountryCodeNavigation)
                .Include(a => a.IdNavigation.LegalEntityPhones)
                .ThenInclude(lep => lep.PhoneNumber)
                .Include(a => a.IdNavigation.LegalEntityEmails)
                .Join(ctx.VAccountStatuses, act => act.AccountNum, vact => vact.AccountNum,
                    (act, vact) => new { Account = act, Active = vact.AccountStatus == "Active" })
                .Where(a => EF.Functions.Like(a.Account.IdNavigation.FullName, $"%{searchString}%") &&
                            (activeOnly == false || a.Active))
                .Select(a => a.Account)
                .ToListAsync();
            return matchingAccounts;
        }

        [Authorize]
        public async Task<List<AccountWatch>> GetAllAccountWatches(string accountNum,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.AccountWatches
                .Include(i => i.WatchStatusNavigation)
                .Where(r => r.AccountNum == accountNum)
                .ToListAsync();

            return result;
        }
    }
}