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
        [Authorize]
        public async Task<List<Obligee>> SearchObligeesAsync(string searchString, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Obligees
                .Include(o => o.IdNavigation)
                .Where(o => o.IdNavigation.FullName.Contains(searchString))
                .ToListAsync();
        }
        [Authorize]
        public async Task<List<Bond>> GetObligeePrimaryBonds(Guid obligeeId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return await ctx.Bonds.Where(b => b.ObligeeId == obligeeId)
                .Include(b => b.BondType)
                .Include(b => b.AccountNumNavigation)
                .ThenInclude(b => b.IdNavigation)
                .ToListAsync();
        }
        [Authorize]
        public async Task<List<Bond>> GetObligeeSecondaryBonds(Guid obligeeId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.AdditionalObligees.Where(o => o.ObligeeId == obligeeId).Select(o => o.BondNumber)
                .ToListAsync();

            return await ctx.Bonds.Where(b => result.Contains(b.BondNumber))
                .Include(b => b.AccountNumNavigation)
                .ThenInclude(b => b.IdNavigation)
                .ToListAsync();
        }
    }
}
