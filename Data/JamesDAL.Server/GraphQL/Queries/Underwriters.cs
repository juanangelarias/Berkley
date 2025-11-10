namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    public async Task<List<Underwriter>> GetUnderwriters(
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.Underwriters
            .Include(i=>i.IdNavigation)
            .ToListAsync();
    }
}