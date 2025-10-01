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
            // ToDo: We need to figure out how to avoid infinite recursion. in the UI before add recursion in this method
            //       In this moment we are gathering only the first level of recursion.
            
            var ctx = await contextFactory.CreateDbContextAsync();
            var securities = await ctx.Securities
                .Include(i=>i.RoleNavigation)
                .ToListAsync();

            var baseRoles = securities
                .Where(x => x.PrincipalId == principalId)
                .Select(x => x.RoleNavigation)
                .ToList();
            
            var userRoles = securities
                .Where(x => x.PrincipalId == principalId)
                .Select(x => x.Role)
                .ToList();

            var principalIds = securities
                .Where(r => userRoles.Contains(r.Role))
                .Select(s => s.PrincipalId)
                .ToList();

            var response = await ctx.SecurityRoles
                .Where(r => principalIds.Contains(r.Id))
                .ToListAsync();

            response.AddRange(baseRoles);
            response = response.OrderBy(o => o.Id).Distinct().ToList();
            
            return response.OrderBy(o=>o.Ord).ToList();
            
            //NOTE: Entity Framework makes the format string below safe from SQL injection attacks.
            /*var result = await ctx.SecurityRoles.FromSqlInterpolated($"EXEC dbo.GetSecurityRolesByUserId {principalId}")
                                                                .ToListAsync();
            return result.OrderBy(r => r.Ord).ToList(); ;*/
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
        public async Task<List<Employee>> GetAllEmployees([Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var result = await ctx.Employees
                .Where(e => e.Active)
                .OrderBy(e => e.FullName)
                .ToListAsync();
            return result;
        }
    }
}
