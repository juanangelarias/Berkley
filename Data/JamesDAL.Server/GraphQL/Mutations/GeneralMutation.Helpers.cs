using James.Shared;
using Microsoft.AspNetCore.Http;

namespace James.Data.Server.GraphQL.Mutations;

public partial class GeneralMutation
{
    /// <summary>
    /// Retrieves the Employee record based on the current user's nickname/username.
    /// </summary>
    /// <param name="ctx">The database context.</param>
    /// <param name="userShared">The user shared service.</param>
    /// <returns>The Employee record.</returns>
    /// <exception cref="GraphQLException">Thrown if the user is not found in the employee table.</exception>
    private async Task<Employee> GetEmployeeAsync(JamesDatabaseContext ctx, IUserShared userShared)
    {
        var userName = await userShared.GetUserName();

        var employee = await ctx.Employees
                           .FirstOrDefaultAsync(f => f.ActiveDirectoryAccount == userName) ??
                       await ctx.Employees
                           .FirstOrDefaultAsync(f => f.Email!.StartsWith(userName));

        if (employee == null)
            throw new GraphQLException("User not found in employee table.");

        return employee;
    }
}
