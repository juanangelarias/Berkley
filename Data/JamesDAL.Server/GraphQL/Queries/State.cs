using James.Data.Server.Model;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        public async Task<List<State>> GetAllStates([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.States.ToListAsync();
        }
    }
}
