using FileNetP8SoapService;
using HotChocolate.Authorization;
using System.DirectoryServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

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
        public async Task<List<SecurityRole>> GetSecurityRolesByUser(Guid principalId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
                .Include(e => e.UnderwriterIdNavigation)
                .Where(e => !activeOnly || e.Active)
                .OrderBy(e => e.FullName)
                .ToListAsync();
            return result;
        }

        [Authorize]
        [SupportedOSPlatform("windows")]
        public async Task<List<PotentialEmployeeActiveDirectoryInfo>> GetActiveDirectoryUsers(string searchText)
        {

            return await Task.Run(() =>
            {

                var users = new List<PotentialEmployeeActiveDirectoryInfo>();
                if (searchText.Length < 3)
                    //Only search with 3 or more characters
                    return [];
                try
                {
                    //HACK:  Hard coding OU for users.  If this changes, users can be manually added into the database until changes can be implemented
                    using DirectoryEntry searchRoot = new("LDAP://OU=WRB Users,DC=wrbts,DC=ads,DC=wrberkley,DC=com")!;
                    using DirectorySearcher searcher = new(searchRoot)
                    {
                        SearchScope = SearchScope.Subtree,
                        Filter = $"(&(objectCategory=person)(objectClass=user)(samAccountName=*{searchText}*))"
                    };
                    searcher.PropertiesToLoad.Add("samAccountName");
                    searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertiesToLoad.Add("mail");
                    searcher.PropertiesToLoad.Add("givenName");
                    searcher.PropertiesToLoad.Add("initials");
                    searcher.PropertiesToLoad.Add("sn");
                    searcher.PropertiesToLoad.Add("proxyaddresses");
                    searcher.PropertiesToLoad.Add("title");

                    foreach (SearchResult result in searcher.FindAll())
                        try
                        {
                            if (result.Properties.Contains("samAccountName"))
                            {
                                var initials = result.Properties["initials"].Cast<string>().ToArray();
                                var emails = result.Properties["mail"].Cast<string>().ToList();
                                var proxyEmails = result.Properties["proxyaddresses"].Cast<string>();
                                var smtpOptions = proxyEmails.Select(eml => _smtpProxyEmailPattern.Match(eml))
                                    .Where(m => m.Success).Select(m => m.Groups["email"].Value).ToArray();
                                var berkleySuretyOptions = smtpOptions.Where(eml =>
                                    eml.EndsWith("@berkleysurety.com", StringComparison.InvariantCultureIgnoreCase)).ToArray();
                                //Add @berkleysurety.com addresses first
                                foreach (var email in berkleySuretyOptions)
                                    if (emails.Contains(email, StringComparer.InvariantCultureIgnoreCase) == false)
                                        emails.Add(email);
                                //Add non @berkleysurety.com addresses at the end
                                foreach (var email in smtpOptions.Except(berkleySuretyOptions))
                                    if (emails.Contains(email, StringComparer.InvariantCultureIgnoreCase) == false)
                                        emails.Add(email);
                                var siteUser = new PotentialEmployeeActiveDirectoryInfo
                                {
                                    Username = result.Properties["samAccountName"][0].ToString()!,
                                    FullName = result.Properties["cn"][0].ToString()!,
                                    Emails = emails,
                                    Initials =
                                        $"{result.Properties["givenName"]?[0].ToString()?[0]}{(initials.Length > 0 ? initials[0] ?? "" : "")}{result.Properties["sn"]?[0].ToString()?[0]}".ToUpperInvariant(),
                                    Title = result.Properties["title"]?[0].ToString()??""
                                };
                                users.Add(siteUser);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                "Warning, found user without necessary attributes.  Skipping" + ex.Message);
                        }

                    return users;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error querying Active Directory", ex);
                }
            });
        }

        private readonly Regex _smtpProxyEmailPattern = new(@"^smtp\s*:\s*(?<email>\S+@\S+?)\s*$", RegexOptions.IgnoreCase);

    }
}
