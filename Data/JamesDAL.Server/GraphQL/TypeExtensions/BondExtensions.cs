using HotChocolate.Types;
using James.Data.Server.Model;

namespace James.Data.Server.GraphQL.TypeExtensions
{

    [ExtendObjectType(typeof(Bond), IgnoreProperties = new[]{ "BondType", "Agency", "Obligee", "ResponsibleParty" })]
    public class BondExtensions
    {
        public async Task<BondTypeDm> GetBondType([Parent] Bond bond,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == bond.BondType)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                bond.BondType = await ctx.BondTypeDms.SingleAsync(bt => bt.Id == bond.BondTypeId);
            }
            return bond.BondType;
        }
        public async Task<string> GetType([Parent] Bond bond,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await GetBondType(bond, contextFactory);
            return bond.BondType!.BondType;
        }

        public async Task<LegalEntity> GetAgency([Parent] Bond bond,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == bond.Agency)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                bond.Agency = await ctx.LegalEntities.SingleAsync(agc => agc.Id == bond.AgencyId);
            }
            return bond.Agency;
        }
        public async Task<LegalEntity?> GetObligee([Parent] Bond bond,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == bond.Obligee)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                bond.Agency = await ctx.LegalEntities.SingleAsync(agc => agc.Id == bond.ObligeeId);
            }
            return bond.Obligee;
        }
        public async Task<LegalEntity?> GetResponsibleParty([Parent] Bond bond,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (null == bond.ResponsibleParty)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                bond.Agency = await ctx.LegalEntities.SingleAsync(agc => agc.Id == bond.ResponsiblePartyId);
            }
            return bond.ResponsibleParty;
        }
    }
}
