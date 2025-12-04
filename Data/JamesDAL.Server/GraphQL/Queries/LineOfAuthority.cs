using System.Web.Http;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<List<LineOfAuthorityLog>> GetLoaLogsByAccount(string accountNumber,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var response = await ctx.LineOfAuthorityLogs
            .Where(l => l.AccountNum == accountNumber)
            .ToListAsync();
        
        return response;
    }
    
    
}