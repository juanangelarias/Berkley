using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<LegalEntityEmail>> GetLegalEntityEmail(Guid legalEntityId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var result = await ctx.LegalEntityEmails
            .Where(e => e.LegalEntityId == legalEntityId)
            .ToListAsync();

        return result ?? throw new GraphQLException($"No legal entity email exists with Id {legalEntityId}.");
    }

    [Authorize]
    public async Task<List<LegalEntityAddress>> GetLegalEntityAddresses(Guid legalEntityId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var result = await ctx.LegalEntityAddresses
            .Include(i => i.Address)
            .Where(e => e.LegalEntityId == legalEntityId)
            .ToListAsync();

        return result ?? throw new GraphQLException($"No legal entity email exists with Id {legalEntityId}.");
    }

    [Authorize]
    public async Task<List<LegalEntityPhone>> GetLegalEntityPhones(Guid legalEntityId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var result = await ctx.LegalEntityPhones
            .Include(i => i.PhoneNumber)
            .Where(e => e.LegalEntityId == legalEntityId)
            .ToListAsync();

        return result ?? throw new GraphQLException($"No legal entity email exists with Id {legalEntityId}.");
    }
}