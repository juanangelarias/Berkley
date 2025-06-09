using James.Shared;
using James.Shared.Model;
using James.Shared.Server;

namespace James.Test.Shared;

public class TestUserShared : IUserShared
{
    public async Task<SiteUserInfo> GetCurrentUser()
    {
        return await Task.FromResult(_fakeTestUser);
    }

    public async Task<SiteUserInfo> GetUserInfoAsync(string jwtToken)
    {
        return await Task.FromResult(_fakeTestUser);
    }
    private readonly SiteUserInfo _fakeTestUser = new SiteUserInfo
    {
        EntraId = "entraId",
        Username = "TestUser1234",
        FirstName = "first",
        FullName = "full",
        Email = "test@fake.com",
        JWT = "jwt"
    };
}