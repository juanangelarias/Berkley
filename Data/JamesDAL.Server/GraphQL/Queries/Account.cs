using HotChocolate.Authorization;
using HotChocolate.Language;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Account?> GetAccountByNumber(string? accountNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (accountNumber == null)
                return null;
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Accounts.Include(a => a.IdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)
                .Include(a => a.IdNavigation.LegalEntityPhones)
                .ThenInclude(a => a.PhoneNumber)
                .Include(a => a.IdNavigation.LegalEntityEmails)
                .Include(a => a.AgencyNumberNavigation)
                .ThenInclude(ag => ag!.IdNavigation)
                .ThenInclude(agi => agi.LegalEntityAddresses)
                .ThenInclude(agia => agia.Address)
                .Include(a => a.Underwriter)
                .ThenInclude(uw => uw!.IdNavigation)
                .Include(a => a.Agent)
                .ThenInclude(ag => ag!.IdNavigation)
                .Include(a => a.HomeOfficeReviewByNavigation)
                .Include(a => a.BranchReviewByNavigation)
                .FirstOrDefaultAsync(a => a.AccountNum.Trim() == accountNumber.Trim());
        }
        [Authorize]
        public async Task<List<Account>> SearchAccounts(string searchString, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                //TODO: Improve search with fuzzy logic.
                var ctx = await contextFactory.CreateDbContextAsync();
                return await ctx.Accounts
                    .Include(a => a.IdNavigation)
                    .Where(a => a.IdNavigation.FullName.Contains(searchString))
                    .ToListAsync();
            }
            else
            {
                return new List<Account>();
            }
        }
        [Authorize]
        public async Task<InforceAccountLOA> GetAccountActiveLinesOfAuthority(string accountNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            var contractLOA = await ctx.LineOfAuthorityLogs
                .Where(l => l.AccountNum == accountNumber && l.Effective <= DateTime.Today && l.Expiration >= DateTime.Today && l.BondType == "Contract")
                .OrderByDescending(l => l.Created)
                .FirstOrDefaultAsync();

            var commercialLOA = await ctx.LineOfAuthorityLogs
                .Where(l => l.AccountNum == accountNumber && l.Effective <= DateTime.Today && l.Expiration >= DateTime.Today && l.BondType == "Commercial")
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
        public async Task<List<AccountProgram>> GetAccountProgramHistory(string accountNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            return await ctx.AccountPrograms
                .Where(a => a.AccountNum == accountNumber)
                .Include(a => a.AccountProgramStatusHistories)
                .OrderByDescending(a => a.Expiration)
                .ToListAsync();
        }
        [Authorize]
        public async Task<Account?> GetAccountOnly(string? accountNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (accountNumber == null)
                return null;
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Accounts
                .FirstOrDefaultAsync(a => a.AccountNum.Trim() == accountNumber.Trim());
        }
    }
}
