using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class ObligeeMutation
    {
        [Authorize]
        public async Task<Obligee> CreateObligee(Guid id, string fullName, string obligeeType, bool printStatusLetter, string? notes,
            string address1, string? address2, string city, string state, string postalCode, string? phoneNumber, string? email,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            
            LegalEntity newLegalEntity = new LegalEntity()
            {
                Id = id,
                FullName = fullName,
                EntityType = "Obligee",
                IsIndividual = false,
                Parent = id
            };
            Obligee newObligee = new Obligee()
            {
                Id = id,
                PrintStatusLetter = printStatusLetter,
                Type = obligeeType
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
                LegalEntityId = id,
                AddressId = newAddress.Id,
                Type = "Main"
            };
            LegalEntityEmail NewEmail = new LegalEntityEmail()
            {
                Id = Guid.NewGuid(),
                LegalEntityId = id,
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
                LegalEntityId = id,
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
