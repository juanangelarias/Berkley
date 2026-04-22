using HotChocolate.Authorization;
using Microsoft.Data.SqlClient;

namespace James.Data.Server.GraphQL.Mutations;

[MutationType]
public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> SetOnlineSystem(Guid id, string systemName,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        var existing = await ctx.AgentSystemDms
            .FirstOrDefaultAsync(f => f.Id == id);

        if (existing is null)
        {
            ctx.AgentSystemDms
                .Add(new() { Id = id, SystemName = systemName });
        }
        else if (existing.SystemName != systemName)
        {
            throw new($"{systemName} is already in use. Domain Table record values should not be edited, only added or deleted.");
        }

        await ctx.SaveChangesAsync();

        return true;
    }

    [Authorize]
    public async Task<bool> DeleteOnlineSystem(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        try
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var existing = await ctx.AgentSystemDms
                .FirstOrDefaultAsync(f => f.Id == id);

            if (existing is null)
                return false;

            ctx.AgentSystemDms.Remove(existing);
            await ctx.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 547 })
        {
            throw new("Cannot delete record because it is in use.", exception);
        }
    }

    [Authorize]
    public async Task<bool> SetOnlineBondSystem(Guid id, Guid legalEntityId, string systemName, Guid insurerId,
        int writingLimit, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var existing = await ctx.OnlineBondSystems
            .FindAsync(id);

        if (existing is null)
        {
            ctx.OnlineBondSystems
                .Add(new OnlineBondSystem
                {
                    Id = id,
                    LegalEntityId = legalEntityId,
                    SystemName = systemName,
                    InsurerId = insurerId,
                    WritingLimit = writingLimit
                });
        }
        else
        {
            existing.LegalEntityId = legalEntityId;
            existing.SystemName = systemName;
            existing.InsurerId = insurerId;
            existing.WritingLimit = writingLimit;
        }
        
        await ctx.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteOnlineBondSystem(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var existing = await ctx.OnlineBondSystems
            .FindAsync(id);

        if (existing is null)
            return false;
        
        ctx.OnlineBondSystems.Remove(existing);
        await ctx.SaveChangesAsync();
        
        return true;
    }
}