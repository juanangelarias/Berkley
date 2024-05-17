using James.Data.Server.Model;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        public List<State> GetAllStates([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = contextFactory.CreateDbContext();
            return ctx.States.ToList();
        }
    }
}
