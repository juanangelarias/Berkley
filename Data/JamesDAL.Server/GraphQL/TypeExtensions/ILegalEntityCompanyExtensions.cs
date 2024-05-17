// ReSharper disable ConstantConditionalAccessQualifier

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
namespace James.Data.Server.GraphQL.TypeExtensions
{
    [ExtendObjectType(typeof(ILegalEntityCompany))]
    // ReSharper disable once InconsistentNaming
    public class ILegalEntityCompanyExtensions
    {
        public async Task<String> GetFullName([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            //TODO: Change to hydrate company if not already hydrated to prevent multiple lookups.
            JamesDatabaseContext ctx;
            if (string.IsNullOrEmpty(company.IdNavigation?.FullName))
            {
                ctx = await contextFactory.CreateDbContextAsync();
                company.IdNavigation = await ctx.LegalEntities.SingleAsync(le => le.Id == company.Id);
            }
            return company.IdNavigation.FullName;
        }
        public async Task<Address?> GetMainAddress([Parent] ILegalEntityCompany company, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureAddresses(company, contextFactory);

            var existingLea = company.IdNavigation.LegalEntityAddresses.SingleOrDefault(lea => lea.Type == "Main");
            return existingLea?.Address;
        }

        public async Task<LegalEntityAddress[]> GetAddresses([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureAddresses(company, contextFactory);
            return company.IdNavigation.LegalEntityAddresses.ToArray();
        }
        public async Task<PhoneNumber?> GetMainPhoneNumber([Parent] ILegalEntityCompany company, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsurePhoneNumbers(company, contextFactory);
            var existingLep = company.IdNavigation.LegalEntityPhones.SingleOrDefault(lea => lea.Type == "Main");
            return existingLep?.PhoneNumber;
        }
        public async Task<LegalEntityPhone[]> GetPhoneNumbers([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsurePhoneNumbers(company, contextFactory);
            return company.IdNavigation.LegalEntityPhones.ToArray();
        }
        public async Task<string?> GetMainEmailAddress([Parent] ILegalEntityCompany company, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureEmails(company, contextFactory);
            var existingLee = company.IdNavigation.LegalEntityEmails.SingleOrDefault(lea => lea.Type == "Main");
            return existingLee?.EmailAddress;
        }
        public async Task<LegalEntityEmail[]> GetEmailAddresses([Parent] ILegalEntityCompany company,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            await EnsureEmails(company, contextFactory);
            return company.IdNavigation.LegalEntityEmails.ToArray();
        }
        private static async Task EnsureAddresses(ILegalEntityCompany company, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (company.IdNavigation?.LegalEntityAddresses.Any() != true)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                if (null == company.IdNavigation)
                {
                    company.IdNavigation = await ctx.LegalEntities.Include(le => le.LegalEntityAddresses).ThenInclude(lea=>lea.Address)
                        .SingleAsync(le => le.Id == company.Id);
                }
                else if (company.IdNavigation.LegalEntityAddresses.Count == 0)
                {
                    company.IdNavigation.LegalEntityAddresses = await ctx.LegalEntityAddresses.Include(lea => lea.Address)
                        .Where(lea => lea.LegalEntityId == company.Id).ToArrayAsync();
                }
            }
        }

        private static async Task EnsurePhoneNumbers(ILegalEntityCompany company, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (company.IdNavigation?.LegalEntityPhones.Any() != true)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                if (null == company.IdNavigation)
                {
                    company.IdNavigation = await ctx.LegalEntities.Include(le => le.LegalEntityPhones).ThenInclude(lep=>lep.PhoneNumber)
                        .SingleAsync(le => le.Id == company.Id);
                }
                else if (company.IdNavigation.LegalEntityPhones.Count == 0)
                {
                    company.IdNavigation.LegalEntityPhones = await ctx.LegalEntityPhones.Include(lep => lep.PhoneNumber)
                        .Where(lep => lep.LegalEntityId == company.Id).ToArrayAsync();
                }
            }
        }

        private static async Task EnsureEmails(ILegalEntityCompany company, IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            if (company.IdNavigation?.LegalEntityEmails.Any() != true)
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                if (null == company.IdNavigation)
                {
                    company.IdNavigation = await ctx.LegalEntities.Include(le => le.LegalEntityEmails)
                        .SingleAsync(le => le.Id == company.Id);
                }
                else if (company.IdNavigation.LegalEntityEmails.Count == 0)
                {
                    company.IdNavigation.LegalEntityEmails = await ctx.LegalEntityEmails
                        .Where(lee => lee.LegalEntityId == company.Id).ToArrayAsync();
                }
            }
        }

    }
}
