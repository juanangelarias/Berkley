using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Account?> GetAccount(string? accountNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
