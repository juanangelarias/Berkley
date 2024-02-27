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
    }
}
