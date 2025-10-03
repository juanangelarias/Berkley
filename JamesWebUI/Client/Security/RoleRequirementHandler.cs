using James.Shared.Data;
using James.Shared.Model;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using James.Shared;

namespace JamesWebUI.Client.Security
{
    public class RoleRequirementHandler(IDataAccess dataAccess, ILoggingService loggingService) : AuthorizationHandler<RoleRequirement>
    {
        //TODO: Discuss if it would make sense to make a simile of AddEventNotify functionality to log errors if they occur.  
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            try
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    //User is not authenticated, so role cannot be verified.
                    context.Fail(new AuthorizationFailureReason(this, "User is not authenticated."));
                    return;
                }

                await GetEmployeeList();
                if (_employees.Count == 0)
                    loggingService.LogWarning("No employees were returned in the employee list");
                var username = GetUsername(context);
                if (string.IsNullOrWhiteSpace(username))
                    //User is not authenticated, so role cannot be verified.
                    return;
                var employee = _employees.FirstOrDefault(e =>
                    string.Equals(e.ActiveDirectoryAccount, username, StringComparison.OrdinalIgnoreCase));
                if (employee == null)
                {
                    //TODO:Update this to allow non-AD users
                    FailSecurityAttempt(context, requirement, "User is not an employee.",
                        $"RoleRequirementHandler is returning Fail because user is not one of {_employees.Count} employee.");
                    return;
                }

                await DataAccess.GetCacheOrLoadDataAsync(UserRoleLoadItem(employee.Id));
                if (_userRoles.Any(sr =>
                        string.Equals(sr.Role, requirement.Role, StringComparison.InvariantCultureIgnoreCase)))
                    context.Succeed(requirement);
                FailSecurityAttempt(context, requirement, "User was not in role");
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception in HandleRequirementAsync");
            }
        }

        private string? GetUsername(AuthorizationHandlerContext context)
        {
            return context.User.FindFirst("nickname")?.Value ??
                   context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                   context.User.Identity?.Name;
        }
        private void FailSecurityAttempt(AuthorizationHandlerContext context, RoleRequirement requirement, string message, string? details = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(message, "Message must be non-null and non-empty");
            details = details ?? message;
            var username = GetUsername(context)??"null user";
            loggingService.LogInformation(message, details, StandardLoggingCategories.Security, new Dictionary<string, string> { {"Username", username}, {"role", requirement.Role} });
            context.Fail(new AuthorizationFailureReason(this, message));
        }

        private IDataAccess DataAccess { get; } = dataAccess;

        private List<SecurityRole> _userRoles = null!;
        private IDataAccessResult<List<SecurityRole>> _loadUserRolesResult = null!;
        private LoadItem UserRoleLoadItem(Guid userId) =>
            new()
            {
                Key = $"srls{userId}",
                AsyncLoadTask = async () => _loadUserRolesResult = await DataAccess.GetSecurityRolesByUserId(userId),
                CacheLoadTask = cache => _loadUserRolesResult = new DataAccessResult<List<SecurityRole>> { Data = (List<SecurityRole>)cache! },
                ResultVariable = () => _loadUserRolesResult,
                AfterLoad = () =>
                {
                    Debug.Assert(_loadUserRolesResult.Data != null);
                    //Add after load code here
                    _userRoles = _loadUserRolesResult.Data!;
                },
                UseBrowserStorageIfAvailable = false
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
                AfterLoad = () => _employees = _loadEmployeesResult.Data!,
                UseBrowserStorageIfAvailable = false
            };

        private Task? _employeeLoadTask;
        private async Task GetEmployeeList()
        {
            if (_employees == null!)
                await (_employeeLoadTask ??= DataAccess.GetCacheOrLoadDataAsync(EmployeesLoadItem));
        }
    }
}
