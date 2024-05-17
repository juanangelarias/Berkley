using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;

namespace James.Data.Client
{
    public class ClientDataAccess : IDataAccess
    {
        private readonly IJamesClient _jamesClient;
        public ClientDataAccess(IJamesClient jamesClient)
        {

        }
        public async Task<List<Account>> GetAgencyAccounts(string agencyNumber)
        {
            var result = await _jamesClient.AgencyAccounts.ExecuteAsync(agencyNumber); 
            var accountList = new List<Account>((IEnumerable<Account>) result.Data.AgencyAccounts.Select(acnt=>ThisToThat.ToEntityType<Account>(acnt)));
            return accountList;
        }
    }
}
