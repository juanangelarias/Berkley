using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.TypeExtensions
{
    [ExtendObjectType(typeof(AgencyLicense), IgnoreProperties = new []{"Insurer"}, IgnoreFields = null)]
    public class AgencyLicenseExtensions
    {
        public async Task<Insurer> Insurer([Parent] AgencyLicense license,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (license.Insurer == null)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var insurer = (await ctx.Insurers.Include(i=>i.IdNavigation).SingleOrDefaultAsync(i => i.Id == license.InsurerId));
                if (insurer == null)
                    throw new GraphQLException(
                        $"AgencyLicense Id {license.Id} has an InsurerId of {license.InsurerId}, which has no corresponding insurer");
                license.Insurer = insurer;
            }

            return license.Insurer;
        }
    }
}
