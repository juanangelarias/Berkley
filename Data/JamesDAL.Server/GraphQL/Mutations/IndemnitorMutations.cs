using HotChocolate.Authorization;
using James.Data.Server.Exceptions;
using James.Shared.Dto;
using James.Shared.EnumTypes;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> SetIndemnitors(IndemnityDto indemnity,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        try
        {
            Console.WriteLine(ctx.ChangeTracker.AutoDetectChangesEnabled);
            
            var actualIndemnitors = await ctx.Indemnitors
                .Where(i =>
                    i.AccountNum == indemnity.AccountNum &&
                    i.AgreementType == indemnity.AgreementType &&
                    i.AgreementDate == indemnity.AgreementDate)
                .Select(s=> s.Id)
                .ToListAsync();
            
            var toDelete = actualIndemnitors
                .Except(indemnity.Details.Select(d => d.Id))
                .ToList();
            
            if(toDelete.Count > 0)
            {
                var indemnitorsToDelete = ctx.Indemnitors
                    .Where(i => toDelete.Contains(i.Id));
                
                ctx.Indemnitors.RemoveRange(indemnitorsToDelete);
                await ctx.SaveChangesAsync();
            }
            
            foreach (var detail in indemnity.Details)
            {
                var existing = await ctx.Indemnitors
                    .AsTracking()
                    .FirstOrDefaultAsync(i => i.Id == detail.Id);
                
                if (existing is not null)
                {
                    existing.AgreementDate = indemnity.AgreementDate;
                    existing.AgreementType = indemnity.AgreementType;
                    existing.AgreementForm = indemnity.AgreementForm;
                    //existing.DocuSign = indemnity.DocuSign;         // ToDo: This field need to be added to the table
                    existing.Signatory = detail.Signatory;
                    existing.Title = detail.Title;
                    existing.NetLiquidAssets = detail.NetLiquidAssets;
                    existing.NetWorth = detail.NetWorth;
                    existing.IndemnityAmount = detail.IndemnityAmount;
                    existing.SpouseIndemnitor = detail.SpouseIndemnitor;
                    existing.ExecutionDate = detail.ExecutionDate;

                    var legalEntity = ctx.LegalEntities.Local
                                          .FirstOrDefault(le => le.Id == detail.Id)
                                      ?? await ctx.LegalEntities
                                          .AsTracking()
                                          .FirstOrDefaultAsync(le => le.Id == detail.Id);
                    
                    if (legalEntity is null)
                    {
                        legalEntity = new() { 
                            Id = detail.Id,
                            FullName = detail.FullName,
                            Parent = detail.Id,
                            EntityType = EntityTypes.KeyPersonnel,
                            IsIndividual = false
                        };
                        await ctx.LegalEntities.AddAsync(legalEntity);
                    }
                    else
                    {
                        legalEntity.FullName = detail.FullName;
                    }
                }
                else
                {
                    var legalEntity = new LegalEntity
                    {
                        Id = detail.Id,
                        FullName = detail.FullName,
                        Parent = detail.Id,
                        EntityType = EntityTypes.KeyPersonnel,
                        IsIndividual = true
                    };
                    
                    await ctx.LegalEntities.AddAsync(legalEntity);
                    
                    existing = new()
                    {
                        Id = detail.Id,
                        AccountNum = indemnity.AccountNum,
                        AgreementDate = indemnity.AgreementDate,
                        AgreementType = indemnity.AgreementType,
                        AgreementForm = indemnity.AgreementForm,
                        //DocuSign = indemnity.DocuSign,         // ToDo: This field need to be added to the table
                        Signatory = detail.Signatory,
                        Title = detail.Title,
                        NetLiquidAssets = detail.NetLiquidAssets,
                        NetWorth = detail.NetWorth,
                        IndemnityAmount = detail.IndemnityAmount,
                        SpouseIndemnitor = detail.SpouseIndemnitor,
                        ExecutionDate = detail.ExecutionDate
                    };
                    await ctx.Indemnitors.AddAsync(existing);
                }
            }
            
            var changes = ctx.ChangeTracker.Entries()
                .Count(e => e.State != EntityState.Unchanged);

            Console.WriteLine($"Tracked changes: {changes}");
            
            await ctx.SaveChangesAsync();

            await transaction.CommitAsync();
            
            foreach (var detail in indemnity.Details)
            {
                if (detail.EncryptSpouseTaxId != null)
                    await SetSpouseIndemnitorTaxId(detail.Id, detail.EncryptSpouseTaxId, ctx);
            }
            await ctx.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}\r\n\r\n{e.InnerException?.Message}");
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task SetSpouseIndemnitorTaxId(Guid id, string spouseTaxId, JamesDatabaseContext ctx)
    {
        // ToDo: Encrypt and save!

        /*var indemnitor = ctx.Indemnitors.FirstOrDefault(f => f.Id == id);
        if (indemnitor is null)
            return;

        var encryptedSpouseTaxId = "...."; // Encrypt

        indemnitor.EncryptSpouseTaxId = encryptedSpouseTaxId;

        await ctx.SaveChangesAsync();*/
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

    [Authorize]
    public async Task<bool> DeleteIndemnitorRange(List<Guid> ids,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var indemnitors = ctx.Indemnitors.Where(i => ids.Contains(i.Id)).ToList();
        if (indemnitors.Count != ids.Count)
            throw new NotFoundException();

        ctx.Indemnitors.RemoveRange(indemnitors);
        await ctx.SaveChangesAsync();

        return true;
    }
}