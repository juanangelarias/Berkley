using HotChocolate.Authorization;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server.GraphQL.Mutations;

[MutationType]
public partial class GeneralMutation
{
    /*[Authorize]
    public async Task<bool> SetUserSetting(string key, string? value,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var username = contextAccessor.HttpContext?.User.FindFirst("email_address")?.Value;
        if (null == username)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        var userSetting = await ctx.UserSettings
            .FirstOrDefaultAsync(x => x.Username == username && x.Key == key);

        if (userSetting == null)
        {
            userSetting = new()
            {
                Id = Guid.NewGuid(),
                Username = username,
                Key = key,
                Value = value ?? ""
            };
            ctx.UserSettings.Add(userSetting);
        }
        else
        {
            userSetting.Value = value ?? "";
        }

        await ctx.SaveChangesAsync();
        return true;
    }*/
    
    
    
    /*[Authorize]
    public async Task<bool> SetDefaultUserSetting(string key, string? value, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        var userSetting = await ctx.UserSettings
            .FirstOrDefaultAsync(x => x.Username == "default" && x.Key == key);

        if (userSetting == null)
        {
            userSetting = new()
            {
                Id = Guid.NewGuid(),
                Username = "default",
                Key = key,
                Value = value ?? ""
            };
            ctx.UserSettings.Add(userSetting);
        }
        else
        {
            userSetting.Value = value ?? "";
        }

        await ctx.SaveChangesAsync();
        return true;
    }*/
}