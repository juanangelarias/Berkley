using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public async Task<UserSetting?> GetUserSettings(string userEmail, string key,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var settings = await ctx.UserSettings
            .Where(r => r.Username == userEmail && r.Key == key)
            .FirstOrDefaultAsync();

        return settings;
    }
}