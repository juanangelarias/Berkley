using HotChocolate.Subscriptions;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace JamesWebUI.Server.GraphQL.Mutations
{

    [MutationType]
    public class AgencyMutation
    {

        public async Task<Address> SetAddress(SetAddressInput address,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldAddress = ctx.LegalEntityAddresses
                .Include(a => a.Address)
                .FirstOrDefault(a => a.AddressId == address.AddressId)
                ;

            if (oldAddress == null)
                throw new GraphQLException("Invalid AddressId");

            
            oldAddress.Address.Address1 = address.Address1;
            oldAddress.Address.Address2 = address.Address2;
            oldAddress.Address.Address3 = address.Address3;
            oldAddress.Address.City = address.City;
            oldAddress.Address.StateCode = address.StateCode;
            oldAddress.Address.PostalCode = address.PostalCode;

            ctx.Update(oldAddress);
            try
            {
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                var exception = ex;
            }
            
            //await eventSender.SendAsync(nameof(AgencyMutation.SetAddress), oldAddress.Address);
            await eventSender.SendAsync(nameof(Subscription.OnAddressModified), oldAddress.Address);

            return oldAddress.Address;
        }
        public async Task<AgencyLicense> SetLicense(SetLicenseInput license,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldLicense = ctx.AgencyLicenses.FirstOrDefault(l => l.Id == license.LicenseId);

            if (oldLicense == null)
                throw new GraphQLException("Invalid LicenseId");

            oldLicense.AppointingState = license.AppointingState;
            oldLicense.Appointment = license.Appointment;
            oldLicense.Comments = license.Comments;
            oldLicense.Expiration = license.Expiration;
            oldLicense.InsurerId = license.InsurerId;
            oldLicense.IsResident = license.IsResident;
            oldLicense.LicenseNumber = license.LicenseNumber;
            oldLicense.State = license.State;
            oldLicense.IsActive = license.IsActive;
            oldLicense.Termination = license.Termination;

            ctx.Update(oldLicense);
            ctx.SaveChanges();
            await eventSender.SendAsync(nameof(Subscription.OnLicenseModified), oldLicense);

            return oldLicense;
        }
    }

    public class SetAddressInput
    {
        public Guid AddressId { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string City { get; set; }
        public string? StateCode { get; set; }
        public string? PostalCode { get; set; }
    }
    public class SetLicenseInput
    {
        public Guid LicenseId { get; set; }
        public bool? AppointingState { get; set; }
        public DateOnly? Appointment { get; set; }
        public string? Comments { get; set; }
        public DateOnly? Expiration { get; set; }
        public Guid InsurerId { get; set; }
        public bool IsResident { get; set; }
        public string? LicenseNumber { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? Termination { get; set; }
    }
    
}
