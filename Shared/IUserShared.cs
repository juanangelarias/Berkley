using James.Shared.Model;

namespace James.Shared;

public interface IUserShared
{
    Task<string> GetUserName();
    //public Task<IAuth0UserInfo> GetAuth0UserInfo(string jwtToken);
    public Task<SiteUserInfo> GetCurrentUser();
    //public Task<SiteUserInfo> GetUserInfoAsync(string jwtToken);
}