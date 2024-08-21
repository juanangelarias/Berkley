using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<string> GetBondRequestNumber(string bondNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var bond = await ctx.BondRequests.FirstOrDefaultAsync(b => b.BondNumber == bondNumber);
        if (bond != null)
            return bond.BondRequestNumber;
        var commercialBond = await ctx.BondRequestCommercials.FirstOrDefaultAsync(b => b.BondNumber == bondNumber);
        if (commercialBond == null)
            throw new GraphQLException("No bond with this bond number exists");
        return commercialBond.BondRequestNumber.Trim();
    }
    [Authorize]
    public async Task<string?> GetBondNumber(string bondRequestNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var bond = await ctx.BondRequests.FirstOrDefaultAsync(b => b.BondRequestNumber.Trim() == bondRequestNumber);
        if (bond != null)
            return bond.BondNumber;
        var commercialBond = await ctx.BondRequestCommercials.FirstOrDefaultAsync(b => b.BondRequestNumber == bondRequestNumber);
        if (commercialBond == null)
            throw new GraphQLException("No bond with this bond number exists");
        return commercialBond.BondNumber;
    }
}