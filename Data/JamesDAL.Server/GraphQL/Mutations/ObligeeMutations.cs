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
            var ctx = await contextFactory.CreateDbContextAsync();
            
            LegalEntity newLegalEntity = new LegalEntity()
            {
                Id = id,
                FullName = fullName,
                EntityType = "Obligee",
                IsIndividual = false,
                Parent = id
            };
            ctx.LegalEntities.Add(newLegalEntity);

            Obligee newObligee = new Obligee()
            {
                Id = id,
                PrintStatusLetter = printStatusLetter,
                Type = obligeeType,
                Notes = notes
            };
            ctx.Obligees.Add(newObligee);

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

            ctx.Addresses.Add(newAddress);
            ctx.LegalEntityAddresses.Add(newLEAddress);

            if (null != email)
            {
            LegalEntityEmail NewEmail = new LegalEntityEmail()
            {
                Id = Guid.NewGuid(),
                LegalEntityId = id,
                EmailAddress = email,
                Type = "Main"
            };
                ctx.LegalEntityEmails.Add(NewEmail);
            }
            if (null != phoneNumber)
            {
            PhoneNumber newPhoneNumber = new PhoneNumber()
            {
                Id = Guid.NewGuid(),
                CountryCode = "1",
                MainNumber = phoneNumber
            };

            LegalEntityPhone newLEPhone = new LegalEntityPhone()
            {
                LegalEntityId = id,
                    PhoneNumberId = newPhoneNumber.Id,
                    Type = "Main"
            };
            ctx.PhoneNumbers.Add(newPhoneNumber);
            ctx.LegalEntityPhones.Add(newLEPhone);
            }

            var result = await ctx.SaveChangesAsync();

            return newObligee;
        }
    }
}
