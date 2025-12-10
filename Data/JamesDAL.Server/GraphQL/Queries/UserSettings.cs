using HotChocolate.Authorization;
using James.Shared;
using James.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<UserSetting?> GetUserSetting(string key,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
        if (null == username)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");
        
        var settings = await ctx.UserSettings
            .FirstOrDefaultAsync(r => r.Username == username && r.Key == key);

        return settings ?? await ctx.UserSettings
            .FirstOrDefaultAsync(r => r.Username == "default" && r.Key == key);
    }
}