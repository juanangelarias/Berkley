using HotChocolate.Types;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HotChocolatePOC
{
    //[QueryType]
    public  class Query
    {
        public Account? GetAccount(string? accountNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (accountNumber == null)
                return null;
            var ctx = contextFactory.CreateDbContext();
            return ctx.Accounts.Include(a => a.IdNavigation)
                .Include(a => a.IdNavigation.LegalEntityAddresses)
                .ThenInclude(a => a.Address)
                .Include(a => a.IdNavigation.LegalEntityPhones)
                .ThenInclude(a => a.PhoneNumber)
                .Include(a => a.IdNavigation.LegalEntityEmails)
                .Include(a => a.AgencyNumberNavigation)
                .ThenInclude(ag => ag.IdNavigation)
                .ThenInclude(agi => agi.LegalEntityAddresses)
                .ThenInclude(agia => agia.Address)
                .Include(a => a.Underwriter)
                .ThenInclude(uw => uw.IdNavigation)
                .Include(a => a.Agent)
                .ThenInclude(ag => ag.IdNavigation)
                .Include(a => a.HomeOfficeReviewByNavigation)
                .Include(a => a.BranchReviewByNavigation)
                .FirstOrDefault(a => a.AccountNum.Trim() == accountNumber.Trim());
        }

        public string[] GetFilteredAccountNumbers([Service] IDbContextFactory<JamesDatabaseContext> contextFactory, 
            string? filter, bool allowSmallFilters = false)
        {
            var ctx = contextFactory.CreateDbContext();
            if (!allowSmallFilters && (filter?.Length ?? 0) < 3)
                throw new GraphQLException(
                    "Filter must be more than 3 characters, or allowSmallFilters must be set to true.");
            if (string.IsNullOrWhiteSpace(filter))
                return ctx.Accounts.OrderBy(a=>a.AccountNum).Select(a => a.AccountNum).ToArray();
            return ctx.Accounts.Where(a => a.AccountNum.Contains(filter)).Select(a => a.AccountNum).ToArray();
        }
    }
}
