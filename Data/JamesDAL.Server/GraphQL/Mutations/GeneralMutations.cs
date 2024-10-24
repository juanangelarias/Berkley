using HotChocolate.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class GeneralMutations
    {
        [Authorize]
        public async Task<bool> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {

                Address NewAddress = new Address()
                {
                    Id = addressId,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    StateCode = stateCode,
                    PostalCode = postalCode
                };

                var ctx = await contextFactory.CreateDbContextAsync();

                ctx.Addresses.Add(NewAddress);

                var newLEAddress = new LegalEntityAddress
                {
                    LegalEntityId = legalEntityId,
                    AddressId = addressId,
                    Type = addressType
                };

                ctx.LegalEntityAddresses.Add(newLEAddress);
                await ctx.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [Authorize]
        public async Task<bool> CreatePhoneNumber(Guid phoneId, string countryCode, string mainNumber, string? extension,
            Guid legalEntityId, string phoneType, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                PhoneNumber newNumber = new PhoneNumber()
                {
                    Id = phoneId,
                    CountryCode = countryCode,
                    MainNumber = mainNumber,
                    Extension = extension
                };

                LegalEntityPhone lePhone = new LegalEntityPhone()
                {
                    LegalEntityId = legalEntityId,
                    PhoneNumberId = phoneId,
                    Type = phoneType
                };

                var ctx = await contextFactory.CreateDbContextAsync();

                ctx.PhoneNumbers.Add(newNumber);
                ctx.LegalEntityPhones.Add(lePhone);
                await ctx.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [Authorize]
        public async Task<bool> DeleteAddress(Guid addressId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var address = await ctx.Addresses.SingleOrDefaultAsync(x => x.Id == addressId);
                var leAddress = await ctx.LegalEntityAddresses.SingleOrDefaultAsync(x => x.AddressId == addressId);

                if (leAddress != null)
                {
                    ctx.LegalEntityAddresses.Remove(leAddress);
                }
                if (address != null)
                {
                    ctx.Addresses.Remove(address);
                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [Authorize]
        public async Task<bool> DeletePhoneNumber(Guid phoneId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var phone = await ctx.PhoneNumbers.SingleOrDefaultAsync(x => x.Id == phoneId);
                var lePhone = await ctx.LegalEntityPhones.SingleOrDefaultAsync(x => x.PhoneNumberId == phoneId);

                if (lePhone != null)
                {
                    ctx.LegalEntityPhones.Remove(lePhone);
                }
                if (phone != null)
                {
                    ctx.PhoneNumbers.Remove(phone);
                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
