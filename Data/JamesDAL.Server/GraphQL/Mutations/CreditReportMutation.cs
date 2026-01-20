using James.Data.Server.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace James.Data.Server.GraphQL.Mutations;

[MutationType]
public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> SetCreditReport(Guid id, string creditReportAgency, string accountNum, DateTime pulledDate, string rating, string definition,
        string remarks, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var record = ctx.CreditReportHistories
            .FirstOrDefault(r => r.Id == id &&
                r.CreditReport == creditReportAgency);

        if (record != null)
        {
            record.AccountNum = accountNum;
            record.CreditReport = creditReportAgency;
            record.Pulled = pulledDate;
            record.Rating = rating;
            record.Definition = definition;
            record.Remarks = remarks;
        }
        else
        {
            var creditReport = new CreditReportHistory
            {
                Id = Guid.NewGuid(),
                CreditReport = creditReportAgency,
                AccountNum = accountNum,
                Pulled = pulledDate,
                Rating = rating,
                Definition = definition,
                Remarks = remarks
            };
            
            ctx.CreditReportHistories.Add(creditReport);
        }

        await ctx.SaveChangesAsync();
        
        return true;
    }

    [Authorize]
    public async Task<bool> DeleteCreditReport(Guid id,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var record = await ctx.CreditReportHistories
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if (record == null)
            throw new NotFoundException("Credit report not found");
        
        ctx.CreditReportHistories.Remove(record);
        await ctx.SaveChangesAsync();
        
        return true;
    }
}