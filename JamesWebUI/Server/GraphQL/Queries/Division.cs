using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.Queries
{
    public partial class Query
    {
        public List<DivisionDm> GetDivisions([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            return ctx.DivisionDms.ToList();
        }
    }
}
