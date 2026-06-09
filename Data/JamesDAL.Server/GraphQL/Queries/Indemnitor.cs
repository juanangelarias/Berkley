using HotChocolate.Authorization;
using James.Shared.Dto;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<IndemnityDto>> GetIndemnitorsByAccount(string accountNum,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var indemnitors = await ctx.Indemnitors
            .Include(i => i.IdNavigation)
            .Where(i => i.AccountNum == accountNum)
            .ToListAsync();

        var indemnities = indemnitors
            .Where(r => r.AccountNum == accountNum)
            .OrderBy(o => o.AccountNum)
            .ThenBy(o => o.AgreementType)
            .ThenBy(o => o.AgreementDate)
            .DistinctBy(d => new { d.AccountNum, d.AgreementType, d.AgreementDate })
            .Select(s => new IndemnityDto
            {
                AccountNum = s.AccountNum,
                AgreementType = s.AgreementType,
                AgreementDate = s.AgreementDate,
                AgreementForm = s.AgreementForm,
                DocuSign = true, // ToDo: This field needs to be created in the table
                Details = []
            })
            .ToList();

        foreach (var ind in indemnities)
        {
            ind.Details = indemnitors
                .Where(i =>
                    i.AccountNum == ind.AccountNum &&
                    i.AgreementType == ind.AgreementType &&
                    i.AgreementDate == ind.AgreementDate)
                .Select(s => new IndemnityDetailDto
                {
                    Id = s.Id,
                    FullName = s.IdNavigation.FullName,
                    NetLiquidAssets = s.NetLiquidAssets,
                    NetWorth = s.NetWorth,
                    IndemnityAmount = s.IndemnityAmount,
                    SpouseIndemnitor = s.SpouseIndemnitor,
                    EncryptSpouseTaxId = s.EncryptSpouseTaxId,
                    Signatory = s.Signatory,
                    Title = s.Title,
                    ExecutionDate = s.ExecutionDate
                })
                .ToList();
        }

        return indemnities;
    }
}