using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.Queries
{
    public partial class Query
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
    }
}
