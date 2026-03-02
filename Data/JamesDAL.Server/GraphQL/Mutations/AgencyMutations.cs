using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using James.Shared;
using System.Diagnostics;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class AgencyMutation
    {
        [Authorize]
        public async Task<Agency> CreateAgency(Guid parentId, string fullName, string branch, bool nasbp, bool w9,
            bool need1099, bool profitSharing, string address1, string address2, string address3, string city,
            string stateCode, string postalCode, string billingAddress1, string billingAddress2, string billingAddress3,
            string billingCity, string billingStateCode, string billingPostalCode,
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

        public async Task<AgencyInventory> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity,
            string documentType, string? addressee, Guid addressId, string address1, string? address2, string? address3,
            string city, string? stateCode, string? postalCode, [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
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
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception saving agency inventory to database", "Database");
            }

            return oldInventory;
        }

        public async Task<AgencyLicense> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId,
            bool appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
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
        public async Task<bool> CreateLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory,
            [Service] ILoggingService loggingService)
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
        public async Task<bool> CreateAgencyStatusLog(Guid id, string agencyNumber, DateTime effective,
            string oldStatus, string newStatus, Guid changedBy, string? comments,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var newStatusLog = new AgencyStatusLog()
                {
                    Id = id,
                    AgencyNumber = agencyNumber,
                    Effective = effective,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    Comments = comments,
                    ChangedBy = changedBy
                };
                var agency = ctx.Agencies.FirstOrDefault(a => a.AgencyNumber == agencyNumber);
                agency!.Status = newStatus;
                ctx.Add(newStatusLog);
                ctx.Update(agency);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> CreateAgencyPOA(Guid poaId, Guid insurerId, Guid agencyId, int limit,
            string? referenceNumber, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var newPoa = new PowerOfAttorney()
                {
                    Id = poaId,
                    InsurerId = insurerId,
                    AgencyId = agencyId,
                    Limit = limit,
                    ReferenceNumber = referenceNumber,
                    FirstIssued = firstIssued,
                    CurrentIssued = currentIssued,
                    Status = status,
                    Comments = comments
                };
                ctx.Add(newPoa);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> CreateAgencyPOADocumentLink(Guid poaId, Guid? imagingDocumentId,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var poa = ctx.PowerOfAttorneys.First(p => p.Id == poaId);
                poa.ImagingId = imagingDocumentId;
                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> CreateAgencyLicenseDocumentLink(Guid licenseId, Guid? imagingDocumentId,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var license = ctx.AgencyLicenses.First(p => p.Id == licenseId);
                license.ImagingId = imagingDocumentId;
                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> DeleteAgencyPOA(Guid poaId, [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var poa = ctx.PowerOfAttorneys.FirstOrDefault(p => p.Id == poaId);
                if (poa == null)
                    return false;

                var poaDocs = ctx.PowerOfAttorneyDocumentStatuses.Where(p => p.Poaid == poaId).ToList();
                ctx.PowerOfAttorneys.Remove(poa);
                foreach (var poaDoc in poaDocs)
                {
                    ctx.PowerOfAttorneyDocumentStatuses.Remove(poaDoc);
                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> CreateAgencyInventory(Guid inventoryId, Guid agencyId, DateTime dateSent, int quantity,
            string documentType, string addressee, string address1, string? address2, string? address3, string city,
            string? stateCode, string? postalCode, Guid approverId, [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            try
            {
                var newAddress = new Address()
                {
                    Id = Guid.NewGuid(),
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    StateCode = stateCode,
                    PostalCode = postalCode,
                };
                //TODO: Use a proper userID via Auth0?
                var newInventory = new AgencyInventory()
                {
                    Id = inventoryId,
                    AgencyId = agencyId,
                    Sent = dateSent,
                    Quantity = quantity,
                    DocumentType = documentType,
                    Addressee = addressee,
                    AddressId = newAddress.Id,
                    Approver = Guid.Parse("67ACEB0B-5C24-4147-9372-FE1F237F2C22")
                };
                ctx.Add(newAddress);
                ctx.Add(newInventory);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> DeleteLicense(Guid licenseId, [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
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

            return success;
            //UNDONE: Support subscriptions with event sender
        }

        [Authorize]
        public async Task<bool> DeleteAgencyInventory(Guid inventoryId, [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            bool success = false;

            var ctx = await contextFactory.CreateDbContextAsync();
            var inventoryToRemove = ctx.AgencyInventories.FirstOrDefault(i => i.Id == inventoryId);

            try
            {
                if (null != inventoryToRemove)
                {
                    ctx.AgencyInventories.Remove(inventoryToRemove);

                    var addressToRemove = ctx.Addresses.FirstOrDefault(f => f.Id == inventoryToRemove!.AddressId);
                    if (addressToRemove != null)
                    {
                        ctx.Addresses.Remove(addressToRemove);
                    }

                    await ctx.SaveChangesAsync(true);
                    success = true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception deleting agency license in database", "Database");
                return false;
            }

            return success;
        }

        [Authorize]
        public async Task<PowerOfAttorney> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit,
            string? referenceNumber, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status,
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
                oldPowerOfAttorney.ReferenceNumber = referenceNumber;

                ctx.Update(oldPowerOfAttorney);
                await ctx.SaveChangesAsync();
                return oldPowerOfAttorney;
            }

            return new PowerOfAttorney();
            //UNDONE: Support subscriptions with event sender
        }

        [Authorize]
        public async Task<PowerOfAttorneyDocumentStatus> SetPowerOfAttorneyDocumentStatus(Guid id, DateTime? requested,
            DateTime? received, Guid documentTypeId, string? comments,
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
        public async Task<bool> SaveCommissionRates(AgencyCommission rate,
            [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            if (rate.AgencyId == Guid.Empty)
                return false;

            rate.ExpireIncluded = rate.Expiration == null ? 0 : 1;

            //Get existing rate id
            var existingRate = await ctx.AgencyCommissions
                .FirstOrDefaultAsync(ac => ac.Id == rate.Id);

            if (existingRate != null)
            {
                existingRate.Minimum = rate.Minimum;
                existingRate.Maximum = rate.Maximum;
                existingRate.Rate = rate.Rate;
            }
            else
            {
                ctx.AgencyCommissions.Add(rate);
            }

            await ctx.SaveChangesAsync();

            return true;
            //UNDONE: Support subscriptions with event sender
        }

        [Authorize]
        public async Task<bool> DeleteAgencyCommissionRate(Guid commRateId,
            [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            var commRateToRemove = ctx.AgencyCommissions
                .FirstOrDefault(f => f.Id == commRateId);

            if (commRateToRemove == null)
                return false;

            ctx.AgencyCommissions.Remove(commRateToRemove);
            await ctx.SaveChangesAsync();
            return true;

        }

        [Authorize]
        public async Task<bool> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId, string? taxId,
            string? npn, bool w9, bool need1099, bool nasbp, string branchKey,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var legalEntity = await ctx.LegalEntities.FirstOrDefaultAsync(l => l.Id == agencyId);
                if (legalEntity == null)
                    return false;

                var agency = await ctx.Agencies.FirstOrDefaultAsync(a => a.Id == agencyId);

                if (null != agency)
                {
                    legalEntity.FullName = agencyName;
                    legalEntity.Parent = parentId;
                    //TODO: Deal with encrypted taxid
                    //legalEntity.TaxIdEncrypted = taxId;
                    agency.NationalProducerNumber = npn;
                    agency.W9 = w9;
                    agency.Need1099 = need1099;
                    agency.Nasbp = nasbp;
                    agency.Branch = branchKey;

                    ctx.Update(legalEntity);
                    ctx.Update(agency);
                    await ctx.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> AgencyLicenseBulkUpdate(List<AgencyLicenseBulk> licenses,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var ids = licenses.Select(s => s.Id).ToList();
                var toUpdate = ctx.AgencyLicenses
                    .Where(r => ids.Contains(r.Id));

                foreach (var license in toUpdate)
                {
                    var input = licenses.First(s => s.Id == license.Id);

                    license.AgencyId = input.AgencyId;
                    license.State = input.State;
                    license.LicenseNumber = input.LicenseNumber;
                    license.IsResident = input.IsResident;
                    license.InsurerId = input.InsurerId;
                    license.Expiration = input.Expiration;
                    license.Comments = input.Comments;
                    license.Appointment = input.Appointment;
                    license.Termination = input.Termination;
                    license.AppointingState = input.AppointingState;
                    license.IsActive = input.IsActive;
                    license.Modified = DateTime.Now;
                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> AgencyLicenseBulkInsert(List<AgencyLicenseBulk> licenses,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var toInsert = licenses
                    .Select(s => new AgencyLicense
                    {
                        Id = s.Id,
                        AgencyId = s.AgencyId,
                        State = s.State,
                        LicenseNumber = s.LicenseNumber,
                        IsResident = s.IsResident,
                        InsurerId = s.InsurerId,
                        Expiration = s.Expiration,
                        Comments = s.Comments,
                        Appointment = s.Appointment,
                        Termination = s.Termination,
                        AppointingState = s.AppointingState,
                        IsActive = s.IsActive,
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    })
                    .ToList();

                await ctx.AddRangeAsync(toInsert);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> AgencyLicenseBulkDelete(List<Guid> licenseIds,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var toDelete = ctx.AgencyLicenses
                    .Where(l => licenseIds.Contains(l.Id))
                    .ToList();

                ctx.RemoveRange(toDelete);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> SetAgencyProfitSharingInfo(Guid agencyId, bool profitSharing,
            int? profitSharingMinimumPremium, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var agency = await ctx.Agencies.FirstOrDefaultAsync(f => f.Id == agencyId);
                if (agency == null)
                    return false;

                agency.ProfitSharing = profitSharing;
                agency.ProfitSharingMinimumPremium = profitSharingMinimumPremium;
                await ctx.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
