using Blazored.LocalStorage;
using James.Shared.Data;
using James.Shared.Model;
using JamesWebUI.Client.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using James.Shared;
using James.Shared.Server;
using System.Security.Claims;

namespace JamesWebUI.Client.Security
{
    public class RoleRequirementHandler(IDataAccess dataAccess, IDataCache dataCache,
        ILocalStorageService localStorageService) : AuthorizationHandler<RoleRequirement>
    {
        //TODO: Discuss if it would make sense to make a simile of AddEventNotify functionality to log errors if they occur.  
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                //User is not authenticated, so role cannot be verified.
                context.Fail(new AuthorizationFailureReason(this, "User is not authenticated."));
                return;
            }
            await GetEmployeeList();
            var username = context.User.FindFirst("nickname")?.Value ??
                           context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                           context.User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username))
                //User is not authenticated, so role cannot be verified.
                return;
            var employee = _employees.FirstOrDefault(e =>  string.Equals(e.ActiveDirectoryAccount, username, StringComparison.OrdinalIgnoreCase));
            if (employee == null)
            {
                //TODO:Update this to allow non-AD users
                context.Fail(new AuthorizationFailureReason(this, "User is not an employee."));
                return;
            }
            await DataCache.GetCacheOrLoadDataAsync(UserRoleLoadItem(employee.Id));
            if (_userRoles.Any(sr =>
                string.Equals(sr.Role, requirement.Role, StringComparison.InvariantCultureIgnoreCase)))
                context.Succeed(requirement);
        }

        private IDataAccess DataAccess { get; } = dataAccess;
        private IDataCache DataCache { get; } = dataCache;
        private ILocalStorageService LocalStorageService { get; } = localStorageService;

        private List<SecurityRole> _userRoles = null!;
        private IDataAccessResult<List<SecurityRole>> _loadUserRolesResult = null!;
        private LoadItem UserRoleLoadItem(Guid userId) =>
            new()
            {
                Key = $"srls{userId}",
                AsyncLoadTask = async () => _loadUserRolesResult = await DataAccess.GetSecurityRolesByUserId(userId),
                CacheLoadTask = cache => _loadUserRolesResult = new DataAccessResult<List<SecurityRole>> { Data = (List<SecurityRole>)cache },
                ResultVariable = () => _loadUserRolesResult,
                AfterLoad = () =>
                {
                    Debug.Assert(_loadUserRolesResult.Data != null);
                    //Add after load code here
                    _userRoles = _loadUserRolesResult.Data!;
                }
            };

        private List<Employee> _employees = null!;
        private IDataAccessResult<List<Employee>> _loadEmployeesResult = null!;
        private LoadItem EmployeesLoadItem =>
            new()
            {
                Key = "Emp",
                AsyncLoadTask = async () => _loadEmployeesResult = await DataAccess.GetAllEmployees(),
                CacheLoadTask = cache => _loadEmployeesResult = new DataAccessResult<List<Employee>> { Data = (List<Employee>)cache! },
                ResultVariable = () => _loadEmployeesResult,
                AfterLoad = () =>  _employees = _loadEmployeesResult.Data!
            };

        private Task? _employeeLoadTask;
        private async Task<List<Employee>> GetEmployeeList()
        {
            if (_employees == null!)
                await (_employeeLoadTask ??= DataCache.GetCacheOrLoadDataAsync(EmployeesLoadItem));
            return _employees!;
        }
    }
}
