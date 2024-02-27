using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.TypeExtensions
{
    [ExtendObjectType(typeof(ILegalEntityIndividual))]
    public class ILegalEntityIndividualExtensions
    {
        public async Task<string?> GetGivenName([Parent] ILegalEntityIndividual individual,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureIdNavigation(individual, contextFactory);
            return individual.IdNavigation.GivenName;
        }
        public async Task<string?> GetMiddleInitial([Parent] ILegalEntityIndividual individual,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureIdNavigation(individual, contextFactory);
            return individual.IdNavigation.MiddleInitial;
        }
        public async Task<string?> GetFamilyName([Parent] ILegalEntityIndividual individual,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureIdNavigation(individual, contextFactory);
            return individual.IdNavigation.FamilyName;
        }

        private static async Task EnsureIdNavigation(ILegalEntityIndividual individual, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (null == individual.IdNavigation)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                individual.IdNavigation = await ctx.LegalEntities.SingleAsync(le => le.Id == individual.Id);
            }
        }
    }
}
