using HotChocolate.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Data.Server.GraphQL.Queries
{
    public partial class Query
    {
        //NOTE: This is a rare method that does not require authorization, as it is used to populate the security roles for the user when they log in.
        public async Task<List<SecurityRole>> GetAllSecurityRoles([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.SecurityRoles.OrderBy(r => r.Ord).ToListAsync();
            return result;
        }

        [Authorize]
        public async Task<List<SecurityRole>>  GetSecurityRolesByUser(Guid principalId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            //NOTE: Entity Framework makes the format string below safe from SQL injection attacks.
            var result = await ctx.SecurityRoles.FromSqlInterpolated($"EXEC dbo.GetSecurityRolesByUserId {principalId}")
                                                                .ToListAsync();
            return result.OrderBy(r => r.Ord).ToList(); ;
        }

        [Authorize]
        public async Task<List<SecurityRoleMember>> GetSecurityRoleMembers(string role, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            //NOTE: Entity Framework makes the format string below safe from SQL injection attacks.
            var result = await ctx.Set<SecurityRoleMember>().FromSqlInterpolated($"EXEC dbo.GetSecurityRoleMembers {role}").ToListAsync();
            return result;
        }

        [Authorize]
        public async Task<List<Employee>> GetEmployees(bool activeOnly, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Employees
                .Include(e=>e.UnderwriterIdNavigation)
                .Where(e => !activeOnly || e.Active)
                .OrderBy(e => e.FullName)
                .ToListAsync();
            return result;
        }
    }
}
