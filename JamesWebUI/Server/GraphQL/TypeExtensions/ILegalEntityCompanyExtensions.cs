using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.TypeExtensions
{
    [ExtendObjectType(typeof(ILegalEntityCompany))]
    public class ILegalEntityCompanyExtensions
    {
        public async Task<String> GetFullName([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            //TODO: Change to hydrate company if not already hydrated to prevent multiple lookups.
            var ctx = await contextFactory.CreateDbContextAsync();
            var existingLe = company.IdNavigation;
            var le = existingLe ?? await ctx.LegalEntities.SingleAsync(le => le.Id == company.Id);
            return le.FullName;
        }
        public async Task<Address?> GetMainAddress([Parent] ILegalEntityCompany company, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var existingLea = company.IdNavigation?.LegalEntityAddresses?.SingleOrDefault(lea => lea.Type == "Main");
            var lea = existingLea ?? await ctx.LegalEntityAddresses.Include(lea => lea.Address).SingleOrDefaultAsync(lea =>
                lea.Type == "Main" && lea.LegalEntityId == company.Id);
            return lea?.Address;
        }

        public async Task<LegalEntityAddress[]> GetAddresses([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var existingLea = company.IdNavigation?.LegalEntityAddresses;
            var lea = existingLea?.ToArray() ?? await ctx.LegalEntityAddresses.Include(lea => lea.Address).Where(lea => lea.LegalEntityId == company.Id).ToArrayAsync();
            return lea;
        }
        public async Task<PhoneNumber?> GetMainPhoneNumber([Parent] ILegalEntityCompany company, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return (await ctx.LegalEntityPhones.Include(lep => lep.PhoneNumber).SingleOrDefaultAsync(lep => lep.Type == "Main" && lep.LegalEntityId == company.Id))?.PhoneNumber;
        }
        public async Task<LegalEntityPhone[]> GetPhoneNumbers([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var existingLep = company.IdNavigation?.LegalEntityPhones;
            var lep = existingLep?.ToArray() ?? await ctx.LegalEntityPhones.Include(lep => lep.PhoneNumber).Where(legalEntityPhone => legalEntityPhone.LegalEntityId == company.Id).ToArrayAsync();
            return lep;
        }
        public async Task<string?> GetMainEmailAddress([Parent] ILegalEntityCompany company, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            return (await ctx.LegalEntityEmails.SingleOrDefaultAsync(lee => lee.Type == "Main" && lee.LegalEntityId == company.Id))?.EmailAddress;
        }
        public async Task<LegalEntityEmail[]> GetEmailAddresses([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var existingLee = company.IdNavigation?.LegalEntityEmails;
            var lee = existingLee?.ToArray() ?? await ctx.LegalEntityEmails.Where(lee => lee.LegalEntityId == company.Id).ToArrayAsync();
            return lee;
        }
    }
}
