using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Data.Server.Model;
using James.Shared.Data;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace James.Data.Server
{
    public class ServerDataAccess(IDbContextFactory<JamesDatabaseContext> contextFactory) : IDataAccess
    {
        private readonly IDbContextFactory<JamesDatabaseContext> _contextFactory = contextFactory;
        public async Task<List<Account>> GetAgencyAccounts(string agencyNumber)
        {
            var ctx = await _contextFactory.CreateDbContextAsync();
            var result = ctx.Accounts.Where(a => a.AgencyNumber == agencyNumber)
                .Include(a => a.IdNavigation)
                .ThenInclude(a => a.LegalEntityAddresses.Where(lea => lea.Type == "Main"))
                .ThenInclude(a => a.Address)
                .Include(a => a.Bonds)
                .ThenInclude(a => a.Obligee)
                .ToList();
            return result;
        }
    }
}
