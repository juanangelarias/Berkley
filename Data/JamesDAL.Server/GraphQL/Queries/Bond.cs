using HotChocolate.Authorization;
using James.Shared.Constants;

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

    [Authorize]
    public async Task<List<BondBlock>> GetBondBlocksByAgency(Guid agencyId, DateTime start, DateTime end, string filter,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        // ToDo: add a filter for issued by
        var response = await ctx.BondBlocks
            .Include(i => i.IssuedByNavigation)
            .Where(b => b.AgencyId == agencyId &&
                        b.Created >= start &&
                        b.Created <= end &&
                        (string.IsNullOrWhiteSpace(filter) ||
                        b.Prefix.ToLower().Contains(filter.ToLower()) ||
                        (b.Comments != null && b.Comments.ToLower().Contains(filter.ToLower())) ||
                        (b.IssuedByNavigation != null &&
                         b.IssuedByNavigation.FullName.ToLower().Contains(filter.ToLower()))))
            .OrderBy(o => o.Prefix)
            .ThenBy(t => t.FirstNumber)
            .ToListAsync();

        return response;
    }

    [Authorize]
    public async Task<List<Bond>> GetBondsByBlock(Guid bondBlockId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var block = await ctx.BondBlocks
            .FirstOrDefaultAsync(b => b.Id == bondBlockId);

        if (block == null)
            throw new GraphQLException($"No bond block exists with id {bondBlockId}");

        // ToDo: If the Bond Number is formatted the same way this block will work.
        // var numbers = new List<string>();
        // for (var i = block.FirstNumber; i <= block.LastNumber; i++)
        // {
        //     numbers.Add($"{block.Prefix}{i:000000000}");
        // }

        // var agBonds = await ctx.Bonds
        //     .Include(i=>i.Obligee)
        //     .Include(i=>i.BondTransactions)
        //     .Where(r => r.AgencyId == block.AgencyId &&
        //                 numbers.Contains(r.BondNumber))
        //     .ToListAsync();

        // ToDo: This will work for any bond number format.
        //       It is not the most efficient way to do this.
        //       One way to manage this would be to have a field in the Bond table (BondBlockId) that points
        //       to the BondBlock that the bond belongs to.

        var agBonds = await ctx.Bonds
            .Include(i => i.Obligee)
            .Include(i => i.BondTransactions)
            .OrderBy(p => p.AgencyId)
            .Where(r => r.AgencyId == block.AgencyId)
            .ToListAsync();

        var bonds = new List<Bond>();
        foreach (var bond in agBonds)
        {
            var (prefix, number) = GetBondNumberParts(bond.BondNumber);
            if (block.Prefix.Trim() == prefix && number >= block.FirstNumber && number <= block.LastNumber)
                bonds.Add(bond);
        }

        return bonds;
    }

    [Authorize]
    public async Task<int> GetNextBondBlockInitialNumber(string prefix,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var last = ctx.BondBlocks
            .OrderBy(o => o.Prefix)
            .ThenByDescending(t => t.FirstNumber)
            .FirstOrDefault(b => b.Prefix.Trim().ToLower() == prefix.Trim().ToLower());

        return last == null
            ? 1
            : last.LastNumber + 1;
    }

    private (string, int) GetBondNumberParts(string bondNumber)
    {
        var prefix = "";
        var number = "";
        foreach (var c in bondNumber)
        {
            if (char.IsDigit(c))
                number += c;
            else
            {
                if (c != ' ')
                    prefix += c;
            }
        }

        return (prefix.Trim(), int.Parse(number));
    }
}