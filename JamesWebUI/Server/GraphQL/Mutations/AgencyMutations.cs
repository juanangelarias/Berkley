using HotChocolate.Subscriptions;
using James.Data.Server.Model;
using James.Shared.Model;
using JamesWebUI.Client.GraphQL;
using JamesWebUI.Server.SharedServices;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace JamesWebUI.Server.GraphQL.Mutations
{

    [MutationType]
    public class AgencyMutation
    {

        public async Task<Address> SetAddress(Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldAddress = ctx.LegalEntityAddresses
                .Include(a => a.Address)
                .FirstOrDefault(a => a.AddressId == addressId)
                ;

            if (oldAddress == null)
                throw new GraphQLException("Invalid AddressId");

            
            oldAddress.Address.Address1 = address1;
            oldAddress.Address.Address2 = address2;
            oldAddress.Address.Address3 = address3;
            oldAddress.Address.City = city;
            oldAddress.Address.StateCode = stateCode;
            oldAddress.Address.PostalCode = postalCode;

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
        public async Task<AgencyLicense> SetLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldLicense = ctx.AgencyLicenses.FirstOrDefault(l => l.Id == licenseId);

            if (oldLicense == null)
                throw new GraphQLException("Invalid LicenseId");

            oldLicense.AppointingState = appointingState;
            oldLicense.Appointment = appointment;
            oldLicense.Comments = comments;
            oldLicense.Expiration = expiration;
            oldLicense.InsurerId = insurerId;
            oldLicense.IsResident = isResident;
            oldLicense.LicenseNumber = licenseNumber;
            oldLicense.State = state;
            oldLicense.IsActive = isActive;
            oldLicense.Termination = termination;

            ctx.Update(oldLicense);
            ctx.SaveChanges();
            await eventSender.SendAsync(nameof(Subscription.OnLicenseModified), oldLicense);

            return oldLicense;
        }
        public async Task<AgencyLicense> CreateLicense(Guid agencyId, Guid? agentId, bool? appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive, 
            [Service]ITopicEventSender eventSender, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            
            var newLicense = new AgencyLicense()
            {
                Id = Guid.NewGuid(),
                AgencyId = agencyId,
                AgentId = agentId,
                State = state,
                LicenseNumber = licenseNumber,
                IsResident = isResident,
                InsurerId = insurerId,
                Expiration = expiration,
                Comments = comments,
                Appointment = appointment,
                Termination = termination,
                AppointingState = appointingState,
                IsActive = isActive
            };
            
            ctx.Add(newLicense);
            ctx.SaveChanges();
            return newLicense;
        }
        public async Task<PowerOfAttorney> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? serial, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, Guid status,
            [Service]ITopicEventSender eventSender, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldPowerOfAttorney = ctx.PowerOfAttorneys.FirstOrDefault(poa => poa.Id == poaId);

            if (oldPowerOfAttorney != null)
            {
                oldPowerOfAttorney.Comments = comments;
                oldPowerOfAttorney.InsurerId = insurerId;
                oldPowerOfAttorney.Limit = limit;
                oldPowerOfAttorney.FirstIssued = firstIssued;
                oldPowerOfAttorney.CurrentIssued = currentIssued;
                oldPowerOfAttorney.Comments = comments;
                oldPowerOfAttorney.Status = status;
                oldPowerOfAttorney.StatusNavigation = ctx.PowerOfAttorneyStatusDms.FirstOrDefault(s => s.Id == status);

                ctx.Update(oldPowerOfAttorney);
                await ctx.SaveChangesAsync();
                return oldPowerOfAttorney;
            }
            return new PowerOfAttorney();
        }
        public async Task<PowerOfAttorneyDocumentStatus> SetPowerOfAttorneyDocumentStatus(Guid id, DateTime? requested, DateTime? received, Guid documentTypeId, string? comments, 
            [Service]ITopicEventSender eventSender, [Service]IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldDocStatus = ctx.PowerOfAttorneyDocumentStatuses.FirstOrDefault(p => p.Id == id);

            if (oldDocStatus != null)
            {
                if (received != null)
                    oldDocStatus.Received = DateOnly.FromDateTime((DateTime)received);
                if (requested != null)
                    oldDocStatus.Requested = DateOnly.FromDateTime((DateTime)requested);
                oldDocStatus.Comments = comments;
                oldDocStatus.DocumentTypeId = documentTypeId;

                ctx.Update(oldDocStatus);
                ctx.SaveChanges();
                return oldDocStatus;
            }
            return new PowerOfAttorneyDocumentStatus();
        }
    }


}
