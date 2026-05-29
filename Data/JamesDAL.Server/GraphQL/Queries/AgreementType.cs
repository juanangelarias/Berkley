using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<AgreementTypeDm>> GetAgreementTypes([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        return await ctx.AgreementTypeDms
            .ToListAsync();
    }
}