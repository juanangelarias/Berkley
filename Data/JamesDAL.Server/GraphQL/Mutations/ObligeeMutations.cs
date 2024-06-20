using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    [Authorize]
    public class ObligeeMutations
    {
        public async Task<Obligee> CreateObligee(string fullName, string obligeeType, bool printStatusLetter, string notes,
            string address1, string address2, string city, string state, string postalCode, string phoneNumber, string email,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            Guid newId = Guid.NewGuid();
            LegalEntity newLegalEntity = new LegalEntity()
            {
                Id = newId,
                FullName = fullName,
                EntityType = "Obligee",
                IsIndividual = false,
                Parent = newId
            };
            Obligee newObligee = new Obligee()
            {
                Id = newId,
                PrintStatusLetter = printStatusLetter
            };
            Address newAddress = new Address()
            {
                Id = Guid.NewGuid(),
                Address1 = address1,
                Address2 = address2,
                City = city,
                StateCode = state,
                PostalCode = postalCode
            };
            LegalEntityAddress newLEAddress = new LegalEntityAddress()
            {
                LegalEntityId = newId,
                AddressId = newAddress.Id,
                Type = "Main"
            };
            LegalEntityEmail NewEmail = new LegalEntityEmail()
            {
                Id = Guid.NewGuid(),
                LegalEntityId = newId,
                EmailAddress = email,
                Type = "Main"
            };
            PhoneNumber newPhoneNumber = new PhoneNumber()
            {
                Id = Guid.NewGuid(),
                CountryCode = "1",
                MainNumber = phoneNumber
            };
            LegalEntityPhone newLEPhone = new LegalEntityPhone()
            {
                LegalEntityId = newId,
                PhoneNumberId = newPhoneNumber.Id
            };

            var ctx = await contextFactory.CreateDbContextAsync();

            ctx.LegalEntities.Add(newLegalEntity);
            ctx.Addresses.Add(newAddress);
            ctx.PhoneNumbers.Add(newPhoneNumber);
            ctx.Obligees.Add(newObligee);
            ctx.LegalEntityAddresses.Add(newLEAddress);
            ctx.LegalEntityPhones.Add(newLEPhone);
            ctx.LegalEntityEmails.Add(NewEmail);

            var result = await ctx.SaveChangesAsync();

            return newObligee;
        }
    }
}
