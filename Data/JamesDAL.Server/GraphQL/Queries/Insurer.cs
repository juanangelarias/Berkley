using James.Data.Server.Model;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        public List<Insurer> GetAllInsurers([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();

            return ctx.Insurers.Include(i => i.IdNavigation).ToList();
        }
    }
}
