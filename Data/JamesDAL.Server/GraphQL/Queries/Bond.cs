using HotChocolate.Authorization;
using James.Shared.Data;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<BondRequestNumberType?> GetBondRequestNumberType(string bondNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var bond = await ctx.BondRequests.FirstOrDefaultAsync(b => b.BondNumber != null && b.BondNumber.Trim() == bondNumber.Trim());
        if (bond != null)
            return new BondRequestNumberType() { BondRequestNumber = bond.BondRequestNumber, Type = "Contract" };
        var commercialBond = await ctx.BondRequestCommercials.FirstOrDefaultAsync(b => b.BondNumber == bondNumber);
        if (commercialBond == null)
            return null;
        return new BondRequestNumberType() { BondRequestNumber = commercialBond.BondRequestNumber.Trim(), Type = "Commercial" };
    }
    [Authorize]
    public async Task<string?> GetBondNumber(string bondRequestNumber, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var bond = await ctx.BondRequests.FirstOrDefaultAsync(b => b.BondRequestNumber.Trim() == bondRequestNumber.Trim());
        if (bond != null)
            return bond.BondNumber;
        var commercialBond = await ctx.BondRequestCommercials.FirstOrDefaultAsync(b => b.BondRequestNumber == bondRequestNumber);
        if (commercialBond == null)
            throw new GraphQLException("No bond with this bond request number exists");
        return commercialBond.BondNumber?.Trim();
    }

    [Authorize]
    public async Task<List<Bond>> SearchBondsByBondNumber(string bondNumberFragment, bool activeOnly, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {

        if (StandardRegularExpressions.BondNumberPattern.IsMatch(bondNumberFragment))
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            //Add what is needed for a search result (Name and address of account)
            var matchingBonds = await ctx.Bonds.Where(b => EF.Functions.Like(b.BondNumber, $"%{bondNumberFragment}%") && (b.Status == "Open" || activeOnly == false))
                .Include(b => b.UnderWriter)
                .ThenInclude(uw => uw.IdNavigation)
                .Include(b => b.AccountNumNavigation)
                .ThenInclude(act => act.IdNavigation)
                .ThenInclude(le => le.LegalEntityAddresses)
                .ThenInclude(lea => lea.Address)
                .ThenInclude(ad => ad.StateCodeNavigation)
                .ThenInclude(sc => sc.CountryCodeNavigation)
                .ToListAsync();
            return matchingBonds;
        }
        return [];
    }
}