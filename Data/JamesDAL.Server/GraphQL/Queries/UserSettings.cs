using HotChocolate.Authorization;
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

    [Authorize]
    public async Task<Dictionary<string, string>> GetAllUserSettings(
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        try
        {
            var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
            if (null == username)
                throw new UnauthorizedAccessException("Must be logged in to get user settings.");
            var ctx = await contextFactory.CreateDbContextAsync();
            var ctx2 = await contextFactory.CreateDbContextAsync();
            var userSettingsTask = ctx.UserSettings.Where(up => up.Username == username)
                .Select(up => new KeyValuePair<string, string>(up.Key, up.Value)).ToListAsync();
            var defaultSettingsTask = ctx2.UserSettings.Where(up => up.Username == "Default")
                .Select(up => new KeyValuePair<string, string>(up.Key, up.Value)).ToListAsync();
            Task[] parallelTasks = [userSettingsTask, defaultSettingsTask];
            await Task.WhenAll(parallelTasks);

            //Begin with defaults
            var settings = defaultSettingsTask.Result.ToDictionary(k => k.Key, v => v.Value);
            //Add user specific settings, overwriting defaults as needed
            foreach (var setting in userSettingsTask.Result)
                settings[setting.Key] = setting.Value;
            return settings;
        }
        catch (Exception ex)
        {
            throw new GraphQLException($"Error when retrieving user settings.", ex);
        }
    }

    [Authorize]
    public async Task<UserInfoDto?> GetUserEmployeeInfo(string username,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();


        #region Recheck

        // ToDo: Once we rethink how we will identify the user, it will be necessary to rewrite this region.
        
        var employee = await ctx.Employees
            .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == username);
        
        if(employee == null) 
            return null;
        
        var index = employee.Email.IndexOf("@");
        var userName = index != -1 
            ? employee.Email.Substring(0, index) 
            : employee.ActiveDirectoryAccount;
        
        #endregion
        
        
        var isUnderWriter = await ctx.Underwriters
            .AnyAsync(a => a.Id == employee.Id);
        
        return new UserInfoDto
        {
            EmployeeId = employee.Id,
            Username = userName,
            FullName = employee.FullName,
            Title = employee.Title ?? "",
            Email = employee.Email ?? "",
            IsUnderwriter = isUnderWriter,
            HomeOfficeApprover = employee.HomeOfficeApprover
        };
    }

    [Authorize]
    public async Task<List<UserLineOfAuthority>> GetUserLOAByDivision(string division,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
        [Service] IHttpContextAccessor contextAccessor)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        var username = contextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
        if (null == username)
            throw new UnauthorizedAccessException("Must be logged in to get user settings.");

        var userId = (await ctx.Employees
            .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == username))?
            .Id;
        
        if(userId == null)
            throw new GraphQLException("User not found in employee table.");
        
        return await ctx.UserLineOfAuthorities
            .Where(f => f.UserId == userId && f.DivisionCode == division)
            .ToListAsync();
    }
}