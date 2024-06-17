using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<List<DivisionDm>> GetDivisions([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.DivisionDms.ToListAsync();
        }
    }
}
