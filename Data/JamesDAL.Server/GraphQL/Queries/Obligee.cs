using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        [Authorize]
        public async Task<Obligee?> GetObligeeById(Guid id, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Obligees
                .Include(o => o.IdNavigation)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        [Authorize]
        public async Task<List<ObligeeTypeDm>> GetObligeeTypes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.ObligeeTypeDms.ToListAsync();
        }
    }
}
