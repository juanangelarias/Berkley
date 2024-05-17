using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Shared.Model;

namespace James.Shared.Data
{
    public interface IDataAccess
    {
        public Task<List<Account>> GetAgencyAccounts(string agencyNumber);
    }
}
