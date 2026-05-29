using HotChocolate.Authorization;
using James.Data.Server.Exceptions;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> SetIndemnitor(Guid id, string accountNum, DateOnly agreementDate, string? agreementType, 
        string? agreementForm, string? signatory, string fullName, string? familyName, string? title, 
        int? netLiquidAssets, int? netWorth, int? indemnityAmount, bool spouseIndemnitor, string? spouseTaxId,
        DateOnly? executionDate, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        using var transaction = await  ctx.Database.BeginTransactionAsync();

        try
        {
            var existing = await ctx.Indemnitors.FirstOrDefaultAsync(i => i.Id == id);
            if (existing is not null)
            {
                existing.AgreementDate = agreementDate;
                existing.AgreementType = agreementType;
                existing.AgreementForm = agreementForm;
                existing.Signatory = signatory;
                existing.Title = title;
                existing.NetLiquidAssets = netLiquidAssets;
                existing.NetWorth = netWorth;
                existing.IndemnityAmount = indemnityAmount;
                existing.SpouseIndemnitor = spouseIndemnitor;
                existing.ExecutionDate = executionDate;
            
                var legalEntity = await ctx.LegalEntities.FirstOrDefaultAsync(le => le.Id == id);
                if (legalEntity is null)
                {
                    legalEntity = new() {Id = id};
                }
            
                legalEntity.FamilyName = familyName;
                legalEntity.FullName = fullName;
            }
            else
            {
                existing = new()
                {
                    Id = id,
                    AccountNum = accountNum,
                    AgreementDate = agreementDate,
                    AgreementType = agreementType,
                    AgreementForm = agreementForm,
                    Signatory = signatory,
                    Title = title,
                    NetLiquidAssets = netLiquidAssets,
                    NetWorth = netWorth,
                    IndemnityAmount = indemnityAmount,
                    SpouseIndemnitor = spouseIndemnitor,
                    ExecutionDate = executionDate
                };

                var legalEntity = new LegalEntity
                {
                    Id = id,
                    FullName = fullName,
                    FamilyName = familyName
                };

                await ctx.Indemnitors.AddAsync(existing);
                await ctx.LegalEntities.AddAsync(legalEntity);
            }

            await ctx.SaveChangesAsync();

            if (spouseTaxId != null)
                await SetSpouseIndemnitorTaxId(id, spouseTaxId, ctx);
            
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task SetSpouseIndemnitorTaxId(Guid id, string spouseTaxId,
        JamesDatabaseContext ctx)
    {
        return;

        // ToDo: Encrypt and save!

        var indemnitor = ctx.Indemnitors.FirstOrDefault(f => f.Id == id);
        if (indemnitor is null)
            return;

        var encryptedSpouseTaxId = "...."; // Encrypt

        indemnitor.EncryptSpouseTaxId = encryptedSpouseTaxId;

        await ctx.SaveChangesAsync();
    }
    
    [Authorize]
    public async Task<bool> DeleteIndemnitor(Guid id, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var indemnitor = ctx.Indemnitors.FirstOrDefault(f => f.Id == id);
        if (indemnitor is null)
            throw new NotFoundException();
        
        ctx.Indemnitors.Remove(indemnitor);
        await ctx.SaveChangesAsync();
        
        return true;
    }
}