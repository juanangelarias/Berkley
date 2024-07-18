using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Collections.Immutable;
using System.Diagnostics;
using James.Shared;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class AgencyMutation
    {
        [Authorize]
        public async Task<Agency> CreateAgency(Guid parentId, string fullName, string branch, bool nasbp, bool w9, bool need1099, bool profitSharing, string address1, string address2, string address3, string city, string stateCode,
            string postalCode, string billingAddress1, string billingAddress2, string billingAddress3, string billingCity, string billingStateCode, string billingPostalCode,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            Guid newId = Guid.NewGuid();
            LegalEntity newLegalEntity = new LegalEntity()
            {
                Id = newId,
                FullName = fullName,
                Parent = parentId,
                EntityType = "Agency",
                IsIndividual = false
            };
            //TODO: Figure out Agency Number
            Agency newAgency = new Agency()
            {
                Id = newId,
                Nasbp = nasbp,
                W9 = w9,
                Need1099 = need1099,
                ProfitSharing = profitSharing,
            };
            Address newMainAddress = new Address()
            {
                Id = Guid.NewGuid(),
                Address1 = address1,
                Address2 = address2,
                Address3 = address3,
                City = city,
                StateCode = stateCode,
                PostalCode = postalCode
            };
            Address newBillingAddress = new Address()
            {
                Id = Guid.NewGuid(),
                Address1 = billingAddress1,
                Address2 = billingAddress2,
                Address3 = billingAddress3,
                City = billingCity,
                StateCode = billingStateCode,
                PostalCode = billingPostalCode
            };
            LegalEntityAddress newLEMainAddress = new LegalEntityAddress()
            {
                LegalEntityId = newId,
                AddressId = newMainAddress.Id
            };
            LegalEntityAddress newLEBillingAddress = new LegalEntityAddress()
            {
                LegalEntityId = newId,
                AddressId = newBillingAddress.Id
            };
            //TODO: Insert the new agency
            return new Agency();
        }
        [Authorize]
        public async Task<Address> SetAddress(Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode, string identifier,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldAddress = await ctx.Addresses
                .FirstOrDefaultAsync(a => a.Id == addressId)
                ;

            if (oldAddress == null)
                throw new GraphQLException("Invalid AddressId");


            oldAddress.Address1 = address1;
            oldAddress.Address2 = address2;
            oldAddress.Address3 = address3;
            oldAddress.City = city;
            oldAddress.StateCode = stateCode;
            oldAddress.PostalCode = postalCode;

            ctx.Update(oldAddress);
            try
            {
                await ctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception saving address to database", "Database");
            }

            await eventSender.SendAsync($"{nameof(Subscription.OnAddressModified)}_{addressId}", new SubscriptionResult<Address>{Identifier = identifier, Result = oldAddress });

            return oldAddress;
        }
        public async Task<AgencyInventory> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity, string documentType, string? addressee, 
            Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldInventory = ctx.AgencyInventories.Where(a => a.Id == inventoryId)
                .Include(a => a.Address)
                .FirstOrDefault();

            if (oldInventory == null)
                throw new GraphQLException("Invalid AgencyInventory ID");

            oldInventory.Sent = sent;
            oldInventory.Quantity = quantity;
            oldInventory.DocumentType = documentType;
            oldInventory.Addressee = addressee;
            Debug.Assert(oldInventory.Address != null, "oldInventory.Address != null");
            oldInventory.Address.Address1 = address1;
            oldInventory.Address.Address2 = address2;
            oldInventory.Address.Address3 = address3;
            oldInventory.Address.City = city;
            oldInventory.Address.StateCode = stateCode;
            oldInventory.Address.PostalCode = postalCode;

            ctx.Update(oldInventory);

            try
            {
                await ctx.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                loggingService.LogException(ex, "Exception saving agency inventory to database", "Database");
            }

            return oldInventory;
        }
        public async Task<AgencyLicense> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, 
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
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
            await ctx.SaveChangesAsync();
            await eventSender.SendAsync(nameof(Subscription.OnLicenseModified), oldLicense);

            return oldLicense;
        }
        [Authorize]
        public async Task<bool> CreateLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var newLicense = new AgencyLicense()
                {
                    Id = licenseId,
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
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception creating agency license in database", "Database");
                return false;
            }
            //UNDONE: Support subscriptions with event sender
        }
        [Authorize]
        public async Task<bool> DeleteLicense(Guid licenseId, [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            bool success = false;

            var ctx = await contextFactory.CreateDbContextAsync();
            var licenseToRemove = ctx.AgencyLicenses.FirstOrDefault(l => l.Id == licenseId);

            if (licenseToRemove != null)
            {
                ctx.AgencyLicenses.Remove(licenseToRemove);
                await ctx.SaveChangesAsync(true);
                success = true;
            }
            //TODO: Handle errors

            return success;
            //UNDONE: Support subscriptions with event sender
        }
        [Authorize]
        public async Task<bool> DeleteAgencyInventory(Guid inventoryId, [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            bool success = false;

            var ctx = await contextFactory.CreateDbContextAsync();
            var inventoryToRemove = ctx.AgencyInventories.FirstOrDefault(i => i.Id == inventoryId);

            if (null != inventoryToRemove)
            {
                ctx.AgencyInventories.Remove(inventoryToRemove);
                await ctx.SaveChangesAsync(true);
                success = true;
            }

            return success;
        }
        [Authorize]
        public async Task<PowerOfAttorney> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? serial, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, Guid status,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
                oldPowerOfAttorney.StatusNavigation = ctx.PowerOfAttorneyStatusDms.First(s => s.Id == status);

                ctx.Update(oldPowerOfAttorney);
                await ctx.SaveChangesAsync();
                return oldPowerOfAttorney;
            }
            return new PowerOfAttorney();
            //UNDONE: Support subscriptions with event sender
        }
        [Authorize]
        public async Task<PowerOfAttorneyDocumentStatus> SetPowerOfAttorneyDocumentStatus(Guid id, DateTime? requested, DateTime? received, Guid documentTypeId, string? comments,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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
                await ctx.SaveChangesAsync();
                return oldDocStatus;
            }
            return new PowerOfAttorneyDocumentStatus();
            //UNDONE: Support subscriptions with event sender
        }

        [Authorize]
        public async Task<bool> SaveCommissionRates(Guid agencyId, AgencyCommission[] rates,
            [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            //Make sure agency Id is set
            foreach (var rate in rates)
                rate.AgencyId = agencyId;

            //Get existing rate ids
            var existingRates = await ctx.AgencyCommissions
                .Where(ac => ac.AgencyId == agencyId).ToListAsync();
            
            //If minimum has changed, Id must be changed to cause Entities to remove/replace it
            //  since Entities will not allow primary keys to be changed.
            foreach (var existingRate in existingRates)
            {
                var updateSource = rates.FirstOrDefault(r => r.Id == existingRate.Id);
                if (updateSource == null) continue;
                if (updateSource.Minimum != existingRate.Minimum)
                    updateSource.Id = Guid.NewGuid();
            }

            var existingIds = existingRates.Select(r => r.Id).ToImmutableList();
            var idsToSave = rates.Select(r => r.Id).ToImmutableList();
            //Delete removed rates
            var deletedRates = existingRates
                .Where(er => !idsToSave.Contains(er.Id));
            ctx.AgencyCommissions.RemoveRange(deletedRates);
            //Update updated rates
            var updatedRates = existingRates
                .Where(er => idsToSave.Contains(er.Id)).ToImmutableList();
            ctx.AgencyCommissions.UpdateRange(updatedRates);
            //TODO: Make sure only relevant columns are updated
            foreach (var updatedRate in updatedRates)
            {
                var updateSource = rates.First(r => r.Id == updatedRate.Id);
                if (updatedRate.Maximum != updateSource.Maximum)
                    updatedRate.Maximum = updateSource.Maximum;
                if (Math.Abs(updatedRate.Rate - updateSource.Rate) > 0.01d)
                    updatedRate.Rate = updateSource.Rate;
                //NOTE: Database trigger should update Modified, not code
            }
            //Add new rates
            var newRates = rates.Where(r => !existingIds.Contains(r.Id)).ToList();
            ctx.AgencyCommissions.AddRange(newRates);

            await ctx.SaveChangesAsync();

            return true;//TODO:Remove if possible.  Might be required to be discovered
            //UNDONE: Support subscriptions with event sender
        }
    }


}
