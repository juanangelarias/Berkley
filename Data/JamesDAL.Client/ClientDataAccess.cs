using James.Data.Client.GraphQL;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;
using StrawberryShake;
using System.Reflection;
using System.Runtime.CompilerServices;
using James.Shared.Dto;
using James.Shared.Imaging;
using ImagingDocumentCategory = James.Shared.Imaging.ImagingDocumentCategory;
using Severity = James.Shared.Model.Severity;
#pragma warning disable CA1305

namespace James.Data.Client
{
    public class ClientDataAccess(IJamesClient jamesClient, ILoggingService logging, 
        IBrowserStorageCache browserStorageCache
    ) : BaseDataAccess(browserStorageCache, logging), IDataAccess
    {
        public async Task<IDataAccessResult<List<Agent>>> SearchAgents(string searchString)
        {
            //TODO: Fix
            return await Task.FromResult( new DataAccessResult<List<Agent>>());
        }
        public async Task<IDataAccessResult<Account>> GetAccountByNumber(string accountNumber)
        {
            return await ExecuteGet<Account>(async () => await jamesClient.GetAccountByNumber.ExecuteAsync(accountNumber),
                "AccountByNumber");

        }
        public async Task<IDataAccessResult<List<AccountProgram>>> GetAccountProgramHistory(string accountNumber)
        {
            return await ExecuteGet<List<AccountProgram>>(async () => await jamesClient.GetAccountProgramHistory.ExecuteAsync(accountNumber),
                "AccountProgramHistory");
        }
        public async Task<IDataAccessResult<InforceAccountLOA>> GetInforceAccountLOAsByAccountNumber(string accountNumber)
        {
            return await ExecuteGet<InforceAccountLOA>(async () => await jamesClient.GetAccountActiveLinesOfAuthority.ExecuteAsync(accountNumber), 
                "AccountActiveLinesOfAuthority");
        }
        public async Task<IDataAccessResult<List<AdditionalRelatedParty>>> GetAdditionalRelatedParties(string? accountNumber)
        {
            return await ExecuteGet<List<AdditionalRelatedParty>>(async () => await jamesClient.GetAdditionalRelatedParties.ExecuteAsync(accountNumber ?? ""),
                "AdditionalRelatedParties");
        }
        public async Task<IDataAccessResult<List<AgencyAccountDto>>> GetAgencyAccounts(string agencyNumber)
        {
            return await ExecuteGet<List<AgencyAccountDto>>(
                async () => await jamesClient.AgencyAccounts.ExecuteAsync(agencyNumber),
                subProperty: "AgencyAccounts");
        }
        
        public async Task<IDataAccessResult<List<AgencyAccountBondDto>>> GetAgencyAccountBonds(string accountNum)
        {
            return await ExecuteGet<List<AgencyAccountBondDto>>(
                async () => await jamesClient.AgencyAccountBonds.ExecuteAsync(accountNum),
                subProperty: "AgencyAccountBonds");
        }

        public async Task<IDataAccessResult<List<Obligee>>> SearchObligees(string searchString)
        {
            return await ExecuteGet<List<Obligee>>(async () => await jamesClient.SearchObligees.ExecuteAsync(searchString), "SearchObligees");
        }
        public async Task<IDataAccessResult<List<Account>>> SearchAccounts(string searchString)
        {
            return await ExecuteGet<List<Account>>(async () => await jamesClient.SearchAccounts.ExecuteAsync(searchString), "SearchAccounts");
        }
        public async Task<IDataAccessResult<Obligee?>> GetObligeeById(Guid id)
        {
            return await ExecuteGet<Obligee?>(async () => await jamesClient.GetObligeeById.ExecuteAsync(id), "ObligeeById");
        }
        public async Task<IDataAccessResult<Obligee?>> GetObligeeByObligeeNumber(string obligeeNumber)
        {
            return await ExecuteGet<Obligee?>(async () => await jamesClient.GetObligeeByObligeeNumber.ExecuteAsync(obligeeNumber), "ObligeeByObligeeNumber");
        }
        public async Task<IDataAccessResult<List<ObligeeTypeDm>>> GetObligeeTypes()
        {
            return await ExecuteGet<List<ObligeeTypeDm>>(async () => await jamesClient.GetObligeeTypes.ExecuteAsync(), "ObligeeTypes");
        }
        public async Task<IDataAccessResult<List<Bond>>> GetObligeePrimaryBonds(Guid obligeeId)
        {
            return await ExecuteGet<List<Bond>>(async () => await jamesClient.GetObligeePrimaryBonds.ExecuteAsync(obligeeId), "ObligeePrimaryBonds");
        }
        public async Task<IDataAccessResult<List<Bond>>> GetObligeeSecondaryBonds(Guid obligeeId)
        {
            return await ExecuteGet<List<Bond>>(async () => await jamesClient.GetObligeeSecondaryBonds.ExecuteAsync(obligeeId), "ObligeeSecondaryBonds");
        }
        public async Task<IDataAccessResult<Agency?>> GetAgencyByAgencyNumber(string agencyNumber)
        {
            return (await ExecuteGet<Agency>(async () => await jamesClient.GetAgencyByAgencyNumber.ExecuteAsync(agencyNumber),
                "AgencyByAgencyNumber"))!;
        }

        public async Task<IDataAccessResult<Agency?>> GetAgencyNameAndNumberById(Guid agencyId)
        {
            var result = await ExecuteGet<Agency>(async () => await jamesClient.GetAgencyNameAndNumberById.ExecuteAsync(agencyId),
                "AgencyByAgencyNumber");
            return result!;
        }

        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId)
        {
            return await ExecuteGet<List<AgencyLicense>>(
                async () => await jamesClient.GetAgencyLicenses.ExecuteAsync(agencyId),
                subProperty: "AgencyLicenses");
        }
        public async Task<IDataAccessResult<List<AgencyInventory>>> GetAgencyInventory(Guid agencyId)
        {

            return await ExecuteGet<List<AgencyInventory>>(
                async () => await jamesClient.GetAgencyInventory.ExecuteAsync(agencyId),
                subProperty: "AgencyInventory");
        }
        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            return await ExecuteGet<List<Insurer>>(
                async () => await jamesClient.AllInsurers.ExecuteAsync(), "AllInsurers");
        }
        public async Task<IDataAccessResult<List<Branch>>> GetAllBranches()
        {
            return await ExecuteGet<List<Branch>>(
                async () => await jamesClient.GetAllBranches.ExecuteAsync(), "AllBranches");
        }
        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            return await ExecuteGet<List<State>>(
                async () => await jamesClient.GetAllStates.ExecuteAsync(), "AllStates");
        }
        public async Task<IDataAccessResult<Address>> GetAddress(Guid addressId)
        {
            return await ExecuteGet<Address>(
                async () => await jamesClient.GetAddress.ExecuteAsync(addressId), "Address");
        }
        public async Task<IDataAccessResult<PhoneNumber>> GetPhoneNumber(Guid phoneId)
        {
            return await ExecuteGet<PhoneNumber>(
                async () => await jamesClient.GetPhoneNumber.ExecuteAsync(phoneId), "PhoneNumber");
        }
        public async Task<IDataAccessResult<List<AddressTypeDm>>> GetAddressTypes()
        {
            return await ExecuteGet<List<AddressTypeDm>>(
                async () => await jamesClient.GetAddressTypes.ExecuteAsync(), "AddressTypes");
        }
        public async Task<IDataAccessResult<List<PhoneTypeDm>>> GetPhoneTypes()
        {
            return await ExecuteGet<List<PhoneTypeDm>>(
                async () => await jamesClient.GetPhoneTypes.ExecuteAsync(), "PhoneTypes");
        }

        public async Task<ISaveDataResult> SetPowerOfAttorneyDocumentLink(Guid poaId, Guid? imagingDocumentId)
        {
            return await ExecuteSave(async () => await jamesClient.SetPoaDocumentLink.ExecuteAsync(new CreateAgencyPOADocumentLinkInput { ImagingDocumentId = imagingDocumentId, PoaId = poaId }));
        }

        public async Task<ISaveDataResult> SetAgencyLicenseDocumentLink(Guid licenseId, Guid? imagingDocumentId)
        {
            return await ExecuteSave(async () => await jamesClient.SetAgencyLicenseDocumentLink.ExecuteAsync(new CreateAgencyLicenseDocumentLinkInput { ImagingDocumentId = imagingDocumentId, LicenseId = licenseId }));
        }

        public async Task<ISaveDataResult> SetAccountCreditReportDocumentLink(Guid? documentId, string accountNum)
        {
            return await ExecuteSave(async () => await jamesClient.SetAccountCurrentCreditReportLink.ExecuteAsync(new SetCurrentCreditReportLinkInput { ImagingDocumentId = documentId, AccountNum = accountNum }));
        }

        public async Task<ISaveDataResult> SetAddress(Address address, string identifier)
        {
            var result = await jamesClient.SetAddress.ExecuteAsync(new SetAddressInput
            {
                AddressId = address.Id,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Address3 = address.Address3,
                City = address.City,
                StateCode = address.StateCode,
                PostalCode = address.PostalCode,
                Identifier = identifier
            });
            return GraphQLSaveResult(result);
        }
        public async Task<ISaveDataResult> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode, Guid legalEntityId, string addressType, string identifier)
        {
            var result = await jamesClient.CreateAddress.ExecuteAsync(new CreateAddressInput()
            {
                AddressId = addressId,
                Address1 = address1,
                Address2 = address2,
                Address3 = address3,
                City = city,
                StateCode = stateCode,
                PostalCode = postalCode,
                LegalEntityId = legalEntityId,
                AddressType = addressType,
                Identifier = identifier
            });
            return GraphQLSaveResult(result);
        }
        public async Task<ISaveDataResult> DeleteAddress(Guid addressId, string identifier)
        {
            var result = await jamesClient.DeleteAddress.ExecuteAsync(new DeleteAddressInput()
            {
                AddressId = addressId,
                Identifier = identifier
            });

            return GraphQLSaveResult(result);

        }
        public async Task<ISaveDataResult> CreatePhoneNumber(Guid phoneId, string? countryCode, string mainNumber, string? extension, Guid legalEntityId, string phoneType)
        {
            //TODO: Make functional
            return new SaveDataResult();
        }
        public async Task<ISaveDataResult> DeletePhoneNumber(Guid phoneId)
        {
            var result = await jamesClient.DeletePhoneNumber.ExecuteAsync(new DeletePhoneNumberInput() { PhoneId = phoneId });

            return GraphQLSaveResult(result);
        }

        public async Task<IDataAccessResult<List<CountryDm>>> GetAllCountries()
        {
            return await ExecuteGet<List<CountryDm>>(
                async () => await jamesClient.GetAllCountries.ExecuteAsync(), "AllCountries");
        }

        public async Task<IDataAccessResult<List<Address>>> GetAllLegalEntityAddresses(Guid legalEntityId)
        {
            return await ExecuteGet<List<Address>>(
                async () => await jamesClient.GetAllLegalEntityAddresses.ExecuteAsync(legalEntityId), "AllLegalEntityAddresses");
        }
        public async Task<IDataAccessResult<List<PhoneNumber>>> GetAllLegalEntityPhoneNumbers(Guid legalEntityId)
        {
            return await ExecuteGet<List<PhoneNumber>>(
                async () => await jamesClient.GetAllLegalEntityPhoneNumbers.ExecuteAsync(legalEntityId), "AllLegalEntityPhoneNumbers");
        }
        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId, int skip, int take)
        {
            throw new NotImplementedException();
            /*return await ExecuteGet<List<Bond>>(
                async () => await jamesClient.GetAgencyBonds.ExecuteAsync(agencyId, skip, take), "AgencyBonds");*/
        }

        public async Task<IDataAccessResult<List<Agent>>> GetAgencyAgents(Guid agencyId)
        {
            return await ExecuteGet<List<Agent>>(
                async () => await jamesClient.GetAgencyAgents.ExecuteAsync(agencyId), "AgencyAgents");
        }
        public async Task<IDataAccessResult<List<AgencyStatusLog>>> GetAgencyStatusLog(string agencyNumber)
        {
            return await ExecuteGet<List<AgencyStatusLog>>(
                async () => await jamesClient.GetAgencyStatusLog.ExecuteAsync(agencyNumber), "AgencyStatusLog");
        }
        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            return await ExecuteGet<List<AgencyStatusDm>>(
                async () => await jamesClient.GetAgencyStatuses.ExecuteAsync(), "AgencyStatuses");
        }

        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            return await ExecuteGet<Agent>(
                async () => await jamesClient.AgentByAgentId.ExecuteAsync(agentId), "AgentByAgentId", "AgentByAgentId");
        }
        public async Task<IDataAccessResult<List<AgencyLocationsDto>>> GetAgencyRelatedParties(Guid agencyId)
        {
            return await ExecuteGet<List<AgencyLocationsDto>>(
                async () => await jamesClient.GetAgencyRelatedParties.ExecuteAsync(agencyId), "AgencyRelatedParties");
        }

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search, bool activeOnly)
        {
            return await ExecuteGet<List<Agency>>(
                async () => await jamesClient.SearchAgencies.ExecuteAsync(search, activeOnly), "SearchAgencies");
        }

        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            return await ExecuteGet<List<PowerOfAttorney>>(async () => await jamesClient.GetAgencyPOAs.ExecuteAsync(agencyId),
                "AgencyPOAs");
        }
        public async Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses()
        {
            return await ExecuteGet<List<PowerOfAttorneyStatusDm>>(async () => await jamesClient.GetAllPoaStatuses.ExecuteAsync(),
                "AllPoaStatuses");
        }
        public async Task<IDataAccessResult<List<InventoryDocumentDm>>> GetAllInventoryDocTypes()
        {
            return await ExecuteGet<List<InventoryDocumentDm>>(async () => await jamesClient.GetAllInventoryDocTypes.ExecuteAsync(),
                "AllInventoryDocTypes");
        }
        public async Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId)
        {
            return await ExecuteGet<List<AgencyCommission>>(async () => await jamesClient.GetAgencyCommissionRates.ExecuteAsync(agencyId),
            "AgencyCommissionRates");
        }
        public async Task<IDataAccessResult<UserProfile>> GetUserProfileByUserName(string userName)
        {
            return await ExecuteGet<UserProfile>(async () => await jamesClient.GetUserProfileByUserName.ExecuteAsync(userName),
                "UserProfileByUserName");
        }
        public async Task<ISaveDataResult> SetAccountGeneralInfo(Guid accountId, string? yearStarted, string? currentManagementYear, string? businessClass,
    string? businessType, string? priorSurety, int? estAnnualPremium)
        {
            var result = await jamesClient.SetAccountGeneralInfo.ExecuteAsync(new SetAccountGeneralInfoInput
            {
                AccountId = accountId,
                YearStarted = yearStarted,
                CurrentManagementYear = currentManagementYear,
                BusinessClass = businessClass,
                BusinessType = businessType,
                PriorSurety = priorSurety,
                EstAnnualPremium = estAnnualPremium
            });
            return GraphQLSaveResult(result);
        }
        public async Task<ISaveDataResult> SetAccountSystems(Guid accountId, string? estimatingSystem, string? estimatingSignoff, string? internalAccountingSystem, bool? interimWips, bool? interimPOCs)
        {
            var result = await jamesClient.SetAccountSystems.ExecuteAsync(new SetAccountSystemsInput
            {
                AccountId = accountId,
                EstimatingSignoff = estimatingSignoff,
                EstimatingSystem = estimatingSystem,
                InterimPOCs = interimPOCs ?? false,
                InterimWips = interimWips ?? false,
                InternalAccountingSystem = internalAccountingSystem
            });
            return GraphQLSaveResult(result);
        }
        public async Task<ISaveDataResult> SetAccountAdditionalInformation(Guid accountId, bool? fullIndemnity, bool? corpIndemnity, bool? personalIndemnity,
            bool? keyManagementLifeInsurance, bool? managementIncentives, bool? fundedBuySell, bool? multipleActiveOwners,
            bool? trackCommAccount, bool? berkleyAffiliate, string? comments)
        {
            var result = await jamesClient.SetAccountAdditionalInformation.ExecuteAsync(new SetAccountAdditionalInformationInput
            {
                AccountId = accountId,
                BerkleyAffiliate = berkleyAffiliate,
                Comments = comments,
                CorpIndemnity = corpIndemnity,
                FullIndemnity = fullIndemnity,
                FundedBuySell = fundedBuySell,
                KeyManagementLifeInsurance = keyManagementLifeInsurance,
                ManagementIncentives = managementIncentives,
                MultipleActiveOwners = multipleActiveOwners,
                PersonalIndemnity = personalIndemnity,
                TrackCommAccount = trackCommAccount
            });
            return GraphQLSaveResult(result);
        }
        public async Task<IDataAccessResult<AccountProgram>> SetAccountProgram(Guid programId, DateTime effective, DateTime expriration, int single, int aggregate,
            string? comments, Guid statusId)
        {
            //TODO: Fix
            return new DataAccessResult<AccountProgram>();
            //return await ExecuteGet<AccountProgram>(async
            //() => await jamesClient.SetAccountProgram.ExecuteAsync(new SetAccountProgramInput
            //{
            //    ProgramId = programId,
            //    Effective = effective,
            //    Expritation = expriration,
            //    Single = single,
            //    Aggregate = aggregate,
            //    Comments = comments,
            //    StatusId = statusId
            //}), graphQlFunctionName: "SetAccountProgram");
            //TODO: Update AccountProgram history log
        }
        public async Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? referenceNumber,
            DateOnly? firstIssued, DateOnly? currentIssued, string? comments, string status)
        {
            return await ExecuteGet<PowerOfAttorney>(async () =>
                await jamesClient.SetPowerOfAttorney.ExecuteAsync(new SetPowerOfAttorneyInput
                {
                    PoaId = poaId,
                    InsurerId = insurerId,
                    Limit = limit,
                    ReferenceNumber = referenceNumber,
                    FirstIssued = firstIssued,
                    CurrentIssued = currentIssued,
                    Comments = comments,
                    Status = status
                }), graphQlFunctionName: "SetPowerOfAttorney");
        }
        public async Task<ISaveDataResult> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity, string documentType, string? addressee,
            Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode)
        {
            return await ExecuteGet<AgencyInventory>(async () =>
            await jamesClient.SetAgencyInventory.ExecuteAsync(new SetAgencyInventoryInput
            {
                InventoryId = inventoryId,
                Sent = sent,
                Quantity = quantity,
                DocumentType = documentType,
                Addressee = addressee,
                AddressId = addressId,
                Address1 = address1,
                Address2 = address2,
                Address3 = address3,
                City = city,
                StateCode = stateCode,
                PostalCode = postalCode
            }), graphQlFunctionName: "SetAgencyInventory");
        }
        public async Task<ISaveDataResult> DeleteLicense(Guid licenseId)
        {
            var result = await jamesClient.DeleteLicense.ExecuteAsync(new DeleteLicenseInput { LicenseId = licenseId });
            return GraphQLSaveResult(result);
        }

        public async Task<ISaveDataResult> DeleteAgencyInventory(Guid inventoryId)
        {
            var result = await jamesClient.DeleteAgencyInventory.ExecuteAsync(new DeleteAgencyInventoryInput { InventoryId = inventoryId });
            return GraphQLSaveResult(result);
        }
        public async Task<ISaveDataResult> SetAgencyCommissionRate(AgencyCommission rate)
        {
            var saveResult = await jamesClient.SaveAgencyCommissionRates.ExecuteAsync(new SaveCommissionRatesInput
            {
                Rate = new AgencyCommissionInput
                {
                    Id = rate.Id,
                    AgencyId = rate.AgencyId,
                    Created = DateTimeOffset.Now, //Created is a required field but not used by the save
                    Modified = DateTimeOffset.Now, //Modified is a required field but not used by the save
                    BondType = rate.BondType,
                    Minimum = rate.Minimum,
                    Maximum = rate.Maximum,
                    Effective = rate.Effective,
                    Expires = rate.Expires,
                    ExpireIncluded = rate.Expires == null ? 0 : 1,
                    Rate = rate.Rate
                }
            });

            return GraphQLSaveResult(saveResult);
        }

        public async Task<ISaveDataResult> DeleteAgencyCommissionRate(Guid commRateId)
        {
            var result = await jamesClient.DeleteAgencyCommissionRate
                .ExecuteAsync(new DeleteAgencyCommissionRateInput{ CommRateId = commRateId });
            
            return GraphQLSaveResult(result);
        }

        public async Task<IDataAccessResult<BondRequestNumberType>> GetBondRequestNumberType(string bondNumber)
        {
            throw new NotImplementedException();
            //return await ExecuteGetString(async () =>
            //await jamesClient.GetBondRequestNumberType.ExecuteAsync(bondNumber));
        }

        public async Task<IDataAccessResult<string?>> GetBondNumber(string bondRequestNumber)
        {
            throw new NotImplementedException();
            //return await ExecuteGetString(async () =>
            //    await jamesClient.GetBondNumber.ExecuteAsync(bondRequestNumber));
        }

        public async Task<IDataAccessResult<List<ImagingType>>> GetAllImagingTypes()
        {
            return await ExecuteGet<List<ImagingType>>(async () => await jamesClient.GetAllImagingTypes.ExecuteAsync(), "AllImagingTypes");
        }

        public async Task<IDataAccessResult<List<VImagingCategoryTabDivisionType>>> GetAllImagingCategoryTabDivisionTypes()
        {
            return await ExecuteGet<List<VImagingCategoryTabDivisionType>>(async () => await jamesClient.GetAllImagingCategoryTabDivisionTypes.ExecuteAsync(), "AllImagingCategoryTabDivisionType");
        }

        

        public IDisposable AddressModified(Guid addressId, Action<SubscriptionResult<Address>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
        {
            var subscriptionToWatch = jamesClient.AddressModified.Watch(addressId.ToString());
            var addressModifiedWatch = new AddressModifiedWatchClass(subscriptionToWatch).SubscribeTo(onNext, onError, onComplete);
            return addressModifiedWatch;
        }
        public IDisposable AddressCollectionModified(Guid legalEntityId, Action<SubscriptionResult<Guid>> onNext,
            Action<Exception>? onError = null, Action? onComplete = null)
        {
            var subscriptionToWatch = jamesClient.AddressCollectionModified.Watch(legalEntityId.ToString());
            var addressCollectionModifiedWatch = new AddressCollectionModifiedWatchClass(subscriptionToWatch).SubscribeTo(onNext, onError, onComplete);
            return addressCollectionModifiedWatch;
        }

        public IDisposable SearchResultReady(string searchTerm, Action<SubscriptionResult<List<JamesSearchResult>>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
        {
            var subscriptionToWatch = jamesClient.SearchResultReady.Watch(searchTerm);
            var searchResultReady =
                new SearchResultReadydWatchClass(subscriptionToWatch).SubscribeTo(onNext, onError, onComplete);
            return searchResultReady;
        }

        public async Task<ISaveDataResult> StartSuperSearch(string searchTerm, SearchOptions options)
        {
            return await ExecuteSave(async () => await jamesClient.SuperSearch.ExecuteAsync(searchTerm, new()
            {
                Account = options.Account,
                ActiveOnly = options.ActiveOnly,
                Agency = options.Agency,
                Agent = options.Agent,
                Bond = options.Bond,
                Obligee = options.Obligee,
                People = options.People,
                PersonalFinancials = options.PersonalFinancials,
                VirtualFile = options.VirtualFile
            }));
        }

        public async Task<IDataAccessResult<List<Account>>> GetIdAccountNumbers()
        {
            return await ExecuteGet<List<Account>>(async () => await jamesClient.GetIdAccountNumbers.ExecuteAsync(), "IdAccountNumbers");
        }

        public async Task<IDataAccessResult<List<Agency>>> GetIdAgencyNumbers()
        {
            return await ExecuteGet<List<Agency>>(async () => await jamesClient.GetIdAgencyNumbers.ExecuteAsync(), "IdAgencyNumbers");
        }

        public async Task<IDataAccessResult<ImagingSearchCriteria>> GetImagingSearchCriteria(string id, ImagingDocumentCategory docCategory, bool useDocCategoryAsCriteria = true)
        {
            var category = Enum.Parse<GraphQL.ImagingDocumentCategory>(docCategory.Name());
            return await ExecuteGet<ImagingSearchCriteria>(async () =>
                await jamesClient.GetImagingSearchCriteria.ExecuteAsync(id, category, useDocCategoryAsCriteria));
        }

        public async Task<IDataAccessResult<List<ImagingDocument>>> SearchDocuments(string imagingId, ImagingDocumentCategory docCategory, string? documentType = null)
        {
            var category = Enum.Parse< GraphQL.ImagingDocumentCategory>(docCategory.Name());
            var result = await ExecuteGet<List<ImagingDocument>>(async () => await jamesClient.GetImagingDocuments.ExecuteAsync(imagingId, category, documentType), "SearchDocuments", "GetImagingDocuments");
            return result;
        }

        public async Task<IDataAccessResult<ImagingDocument?>> GetImagingDocumentsDetails(
            ImagingDocumentCategory docCategory, Guid documentId)
        {
            //This should only be called by the server, because the client cannot access the imaging system.
            throw new NotImplementedException("Imaging details can only be accessed serverside.");
        }

        public async Task<IDataAccessResult<List<SecurityRole>>> GetAllSecurityRoles()
        {
            var result = await ExecuteGet<List<SecurityRole>>(
                async () => await jamesClient.GetAllSecurityRoles.ExecuteAsync(), "AllSecurityRoles");
            return result;
        }

        public async Task<IDataAccessResult<List<SecurityRole>>> GetSecurityRolesByUserId(Guid userId)
        {
            var result = await ExecuteGet<List<SecurityRole>>(
            async () => await jamesClient.GetSecurityRolesByUser.ExecuteAsync(userId), "SecurityRolesByUser");
            return result;
        }

        public async Task<IDataAccessResult<List<SecurityRoleMember>>> GetSecurityRoleMembers(string role)
        {
            var result = await ExecuteGet<List<SecurityRoleMember>>(
                async () => await jamesClient.GetSecurityRoleMembers.ExecuteAsync(role), "SecurityRoleMembers");
            return result;
        }

        public async Task<ISaveDataResult> AddPrincipalToSecurityRole(Guid principalId, string role)
        {
            var result = await ExecuteSave(
                async () => await jamesClient.AddPrincipalToSecurityRole.ExecuteAsync(new AddPrincipalToSecurityRoleInput
                {
                    PrincipalId = principalId,
                    Role = role
                } ), "AddPrincipalToSecurityRole");
            return result;
        }

        public async Task<ISaveDataResult> RemovePrincipalFromSecurityRole(Guid principalId, string role)
        {
            var result = await ExecuteSave(
                async () => await jamesClient.RemovePrincipalFromSecurityRole.ExecuteAsync(new RemovePrincipalFromSecurityRoleInput()
                {
                    PrincipalId = principalId,
                    Role = role
                }), "RemovePrincipalFromSecurityRole");
            return result;
        }

        public async Task<ISaveDataResult> AddSecurityRole(SecurityRole role)
        {
            var result = await ExecuteSave(
                async () => await jamesClient.AddSecurityRole.ExecuteAsync(new AddSecurityRoleInput()
                {
                    Role = role.Role,
                    Description = role.Description,
                    Ord = role.Ord
                }));
            return result;
        }

        public async Task<IDataAccessResult<List<Employee>>> GetAllEmployees()
        {
            var result = await ExecuteGet<List<Employee>>(
                async () => await jamesClient.GetAllEmployees.ExecuteAsync(), "AllEmployees");
            return result;
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyDocumentNameDm>>> GetPoaDocumentNames()
        {
            return await ExecuteGet<List<PowerOfAttorneyDocumentNameDm>>(async () => await jamesClient.GetPOADocumentNames.ExecuteAsync(), "PoaDocumentNames");
        }

        public async Task<IDataAccessResult<PowerOfAttorneyDocumentStatus>> SetPowerOfAttorneyDocumentStatus(Guid id,
            DateTime? requested, DateTime? received, Guid documentTypeId, string? comments)
        {
            return await ExecuteGet<PowerOfAttorneyDocumentStatus>(async () => await jamesClient.SetPowerOfAttorneyDocumentStatus.ExecuteAsync(
                new SetPowerOfAttorneyDocumentStatusInput
                {
                    Id = id,
                    Requested = requested,
                    Received = received,
                    DocumentTypeId = documentTypeId,
                    Comments = comments
                }));
        }

        private sealed class AddressModifiedWatchClass(IObservable<IOperationResult<IAddressModifiedResult>> graphQlSubscription) :
             //IObservable<SubscriptionResult<Address>>,
             IDisposable
        {
            private IDisposable? _internalSubscription;
            public IDisposable SubscribeTo(Action<SubscriptionResult<Address>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
            {
                if (null == onError && null == onComplete)
                    return graphQlSubscription.Subscribe(Conversion(onNext));
                //If onComplete is not null, OnError is required.
                ArgumentNullException.ThrowIfNull(onError, nameof(onError));
                if (null == onComplete)
                    return graphQlSubscription.Subscribe(Conversion(onNext), onError);
                return _internalSubscription = graphQlSubscription.Subscribe(Conversion(onNext), onError, onComplete);
            }

            private static Action<IOperationResult<IAddressModifiedResult>> Conversion(Action<SubscriptionResult<Address>> source)
            {
                return onNextConversion =>
                {
                    ArgumentNullException.ThrowIfNull(onNextConversion.Data, "Subscription Payload");
                    var subscriptionResultAddress = new SubscriptionResult<Address>
                    {
                        Identifier = onNextConversion.Data.OnAddressModified.Identifier,
                        Result = ThisToThat.ToEntityType<Address>(onNextConversion.Data.OnAddressModified.Result)
                    };
                    source.Invoke(subscriptionResultAddress);
                };
            }

            public void Dispose()
            {
                _internalSubscription?.Dispose();
            }
        }
        private sealed class AddressCollectionModifiedWatchClass(IObservable<IOperationResult<IAddressCollectionModifiedResult>> graphQlSubscription) :
            IDisposable
        {
            private IDisposable? _internalSubscription;
            public IDisposable SubscribeTo(Action<SubscriptionResult<Guid>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
            {
                if (null == onError && null == onComplete)
                    return graphQlSubscription.Subscribe(Conversion(onNext));
                //If onComplete is not null, OnError is required.
                ArgumentNullException.ThrowIfNull(onError, nameof(onError));
                if (null == onComplete)
                    return graphQlSubscription.Subscribe(Conversion(onNext), onError);
                return _internalSubscription = graphQlSubscription.Subscribe(Conversion(onNext), onError, onComplete);
            }

            private static Action<IOperationResult<IAddressCollectionModifiedResult>> Conversion(Action<SubscriptionResult<Guid>> source)
            {
                return onNextConversion =>
                {
                    ArgumentNullException.ThrowIfNull(onNextConversion.Data, "Subscription Payload");
                    var subscriptionResultAddress = new SubscriptionResult<Guid>
                    {
                        Identifier = onNextConversion.Data.OnAddressCollectionModified.Identifier,
                        Result = ThisToThat.ToEntityType<Guid>(onNextConversion.Data.OnAddressCollectionModified.Identifier)
                    };
                    source.Invoke(subscriptionResultAddress);
                };
            }

            public void Dispose()
            {
                _internalSubscription?.Dispose();
            }
        }
        private sealed class SearchResultReadydWatchClass(IObservable<IOperationResult<ISearchResultReadyResult>> graphQlSubscription) :
            IDisposable
        {
            private IDisposable? _internalSubscription;
            public IDisposable SubscribeTo(Action<SubscriptionResult<List<JamesSearchResult>>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
            {
                if (null == onError && null == onComplete)
                    return graphQlSubscription.Subscribe(Conversion(onNext));
                //If onComplete is not null, OnError is required.
                ArgumentNullException.ThrowIfNull(onError, nameof(onError));
                if (null == onComplete)
                    return graphQlSubscription.Subscribe(Conversion(onNext), onError);
                return _internalSubscription = graphQlSubscription.Subscribe(Conversion(onNext), onError, onComplete);
            }

            private static Action<IOperationResult<ISearchResultReadyResult>> Conversion(Action<SubscriptionResult<List<JamesSearchResult>>> source)
            {
                return onNextConversion =>
                {
                    ArgumentNullException.ThrowIfNull(onNextConversion.Data, "Subscription Payload");
                    var subscriptionResultSearchResult = new SubscriptionResult<List<JamesSearchResult>>
                    {
                        Identifier = onNextConversion.Data.OnSearchResultReady.Identifier,
                        Result = ThisToThat.ToEntityType<List<JamesSearchResult>>(onNextConversion.Data.OnSearchResultReady.Result)
                    };
                    source.Invoke(subscriptionResultSearchResult);
                };
            }

            public void Dispose()
            {
                _internalSubscription?.Dispose();
            }
        }


        public async Task<ISaveDataResult> CreateAgencyStatusLog(Guid id, string agencyNumber, DateTime effective, string oldStatus, string newStatus, Guid changedBy, string? comments)
        {
            var saveResult = await jamesClient.CreateAgencyStatusLog.ExecuteAsync(new CreateAgencyStatusLogInput
            {
                Id = id,
                AgencyNumber = agencyNumber,
                Effective = effective,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                Comments = comments,
                ChangedBy = changedBy
            });
            return GraphQLSaveResult(saveResult);
        }
        public async Task<ISaveDataResult> CreateAgencyPOA(Guid poaId, Guid insurerId, Guid agencyId, int limit, string? referenceNumber, DateOnly? firstIssued,
            DateOnly? currentIssued, string? comments, string status)
        {
            var saveResult = await jamesClient.CreateAgencyPOA.ExecuteAsync(new CreateAgencyPOAInput
            {
                PoaId = poaId,
                InsurerId = insurerId,
                AgencyId = agencyId,
                Limit = limit,
                ReferenceNumber = referenceNumber,
                FirstIssued = firstIssued,
                CurrentIssued = currentIssued,
                Status = status,
                Comments = comments
            });
            return GraphQLSaveResult(saveResult);
        }
        public async Task<ISaveDataResult> DeleteAgencyPOA(Guid poaId)
        {
            var saveResult = await jamesClient.DeleteAgencyPOA.ExecuteAsync(new DeleteAgencyPOAInput { PoaId = poaId });
            return GraphQLSaveResult(saveResult);
        }
        
        public async Task<ISaveDataResult> AgencyLicenseBulkDelete(List<Guid> licenseIds)
        {
            var result = await jamesClient.AgencyLicenseBulkDelete.ExecuteAsync(new AgencyLicenseBulkDeleteInput
                { LicenseIds = licenseIds });

            return new SaveDataResult
            {
                Errors = result.Errors.Select(s => s.Message).ToArray()
            };
        }
        
        public async Task<ISaveDataResult> AgencyLicenseBulkInsert(List<AgencyLicenseBulk> licenses)
        {
            var result = await jamesClient.AgencyLicenseBulkInsert.ExecuteAsync(new AgencyLicenseBulkInsertInput
            {
                Licenses = licenses
                    .Select(s => new AgencyLicenseBulkInput
                    {
                        Id = s.Id,
                        AgencyId = s.AgencyId,
                        InsurerId = s.InsurerId,
                        Appointment = s.Appointment,
                        AppointingState = s.AppointingState,
                        Comments = s.Comments,
                        Expiration = s.Expiration,
                        IsActive = s.IsActive,
                        IsResident = s.IsResident,
                        LicenseNumber = s.LicenseNumber,
                        State = s.State,
                        Termination = s.Termination
                    })
                    .ToList()
            });

            return new SaveDataResult
            {
                Errors = result.Errors.Select(s => s.Message).ToArray()
            };
        }
        
        public async Task<ISaveDataResult> AgencyLicenseBulkUpdate(List<AgencyLicenseBulk> licenses)
        {
            var result = await jamesClient.AgencyLicenseBulkUpdate.ExecuteAsync(new AgencyLicenseBulkUpdateInput
            {
                Licenses = licenses
                    .Select(s => new AgencyLicenseBulkInput
                    {
                        Id = s.Id,
                        AgencyId = s.AgencyId,
                        InsurerId = s.InsurerId,
                        Appointment = s.Appointment,
                        AppointingState = s.AppointingState,
                        Comments = s.Comments,
                        Expiration = s.Expiration,
                        IsActive = s.IsActive,
                        IsResident = s.IsResident,
                        LicenseNumber = s.LicenseNumber,
                        State = s.State,
                        Termination = s.Termination
                    })
                    .ToList()
            });

            return new SaveDataResult
            {
                Errors = result.Errors.Select(s => s.Message).ToArray()
            };
        }

        public async Task<IDataAccessResult<Obligee>> CreateObligee(Guid id, string fullName, string obligeeType, bool printStatusLetter, string? notes,
            string address1, string? address2, string city, string state, string postalCode, string? phoneNumber, string? email)
        {
            var saveResult = await jamesClient.CreateObligee.ExecuteAsync(new CreateObligeeInput
            {
                Id = id,
                FullName = fullName,
                ObligeeType = obligeeType,
                PrintStatusLetter = printStatusLetter,
                Notes = notes,
                Address1 = address1,
                Address2 = address2,
                City = city,
                State = state,
                PostalCode = postalCode,
                PhoneNumber = phoneNumber,
                Email = email
            });
            return new DataAccessResult<Obligee>() { Data = ThisToThat.ToEntityType<Obligee>(saveResult.Data?.CreateObligee.Obligee) };
        }
        public async Task<ISaveDataResult> CreateLicense(Guid licenseId, Guid agencyId, Guid? agentId,
            bool appointingState,
            string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state,
            bool isActive)
        {
            var saveResult = await jamesClient.CreateLicense.ExecuteAsync(new CreateLicenseInput
            {
                LicenseId = licenseId,
                AgencyId = agencyId,
                AgentId = agentId,
                AppointingState = appointingState,
                Comments = comments,
                Appointment = appointment,
                Expiration = expiration,
                Termination = termination,
                InsurerId = insurerId,
                IsResident = isResident,
                LicenseNumber = licenseNumber,
                State = state,
                IsActive = isActive
            });
            return GraphQLSaveResult(saveResult);
        }
        public async Task<ISaveDataResult> CreateAgencyInventory(Guid inventoryId, Guid agencyId, DateTime dateSent, int quantity, string documentType, string addressee,
            string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode, Guid approverId)
        {
            var saveResult = await jamesClient.CreateAgencyInventory.ExecuteAsync(new CreateAgencyInventoryInput
            {
                InventoryId = inventoryId,
                AgencyId = agencyId,
                DateSent = dateSent,
                Quantity = quantity,
                DocumentType = documentType,
                Addressee = addressee,
                Address1 = address1,
                Address2 = address2,
                Address3 = address3,
                City = city,
                StateCode = stateCode,
                PostalCode = postalCode,
                ApproverId = approverId
            });
            return GraphQLSaveResult(saveResult);
        }

        public async Task<ISaveDataResult> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId, string? taxId, string? npn, bool w9,
            bool need1099, bool nasbp, string branchKey)
        {
            var saveResult = await jamesClient.SetAgencyGeneralInfo.ExecuteAsync(new SetAgencyGeneralInfoInput
            {
                AgencyId = agencyId,
                AgencyName = agencyName,
                ParentId = parentId,
                TaxId = taxId,
                Npn = npn,
                W9 = w9,
                Need1099 = need1099,
                Nasbp = nasbp,
                BranchKey = branchKey
            });
            return GraphQLSaveResult(saveResult);
        }

        public async Task<ISaveDataResult> SetAgencyProfitSharingInfo(Guid agencyId, bool profitSharing,
            int? profitSharingMinimumPremium)
        {
            var result = await jamesClient.SetAgencyProfitSharing.ExecuteAsync(new SetAgencyProfitSharingInfoInput
            {
                AgencyId = agencyId,
                ProfitSharing = profitSharing,
                ProfitSharingMinimumPremium = profitSharingMinimumPremium
            });

            return GraphQLSaveResult(result);
        }
        
        public async Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId,
            Guid? agentId, bool appointingState, string? comments,
            DateOnly? appointment, DateOnly? expiration, DateOnly? termination, Guid insurerId, bool isResident,
            string? licenseNumber, string state, bool isActive)
        {
            return await ExecuteGet<AgencyLicense>(
                async () => await jamesClient.SetAgencyLicense.ExecuteAsync(new SetAgencyLicenseInput
                {
                    LicenseId = licenseId,
                    AgencyId = agencyId,
                    AgentId = agentId,
                    AppointingState = appointingState,
                    Comments = comments,
                    Appointment = appointment,
                    Expiration = expiration,
                    Termination = termination,
                    InsurerId = insurerId,
                    IsResident = isResident,
                    LicenseNumber = licenseNumber,
                    State = state,
                    IsActive = isActive
                }), "SetAgencyLicense");
        }
        
        #region User Settings
        
        public async Task<IDataAccessResult<UserSetting?>> GetUserSetting(string key)
        {
            return await ExecuteGet<UserSetting?>(async () =>
                await jamesClient.GetUserSetting.ExecuteAsync(key), "UserSetting");
        }

        public async Task<IDataAccessResult<Dictionary<string, string>>> GetAllUserSettings()
        {
            var settingList =
                await ExecuteGet<List<KeyValuePair<string, string>>>(
                    async () => await jamesClient.GetAllUserSettings.ExecuteAsync(), "UserSettings");
            return new DataAccessResult<Dictionary<string, string>>
                { Data = settingList.Data?.ToDictionary(), Errors = settingList.Errors };
        }

        public async Task<ISaveDataResult> SetUserSetting(string key, string value)
        {
            return await ExecuteSave(async () =>
                await jamesClient.SetUserSetting.ExecuteAsync(new()
                {
                    Key = key,
                    Value = value ?? ""
                }), "SetUserSetting");
        }

        public async Task<ISaveDataResult> ResetUserSettings()
        {
            return await ExecuteSave(async () =>
                await jamesClient.ResetUserSettings.ExecuteAsync(), "ResetUserSettings");
        }

        public async Task<ISaveDataResult> ResetUserSetting(string key)
        {
            return await ExecuteSave(async () =>
                await jamesClient.ResetUserSetting.ExecuteAsync(new()
                {
                    Key = key
                }), "ResetUserSetting");
        }

        public async Task<ISaveDataResult> SetDefaultUserSetting(string key, string? value)
        {
            return await ExecuteSave(async () =>
                await jamesClient.SetDefaultUserSetting.ExecuteAsync(new()
                {
                    Key = key,
                    Value = value ?? ""
                }), "SetDefaultUserSetting");
        }

        #endregion

        private static string ErrorToString(IClientError error)
        {
            //TODO: Make this better and put somewhere where the code can be shared.
            return
                $"Message: {error.Message}\r\nCode: {error.Code}\r\nException: {error.Exception}\r\nPath: {error.Path}\r\nExtensions: {error.Extensions}";
        }

        private static DataAccessResultString GraphQLResultString(IOperationResult? graphQLResult)
        {
            var resultData = graphQLResult?.Data;
            if (null == resultData)
                return new DataAccessResultString
                { Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
            var data = graphQLResult?.Data?.ToString();
            return new DataAccessResultString
            { Data = data, Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
        }

        /// <summary>
        /// Converts the result of a GraphQL query into a DataAccessResult&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">Type of the query result</typeparam>
        /// <param name="graphQLResult">The result from a GraphQL operation as it is returned from Strawberry Shake</param>
        /// <param name="subProperty">The property of the data result that you want to return in the DataAccessResult&lt;T&gt;.This is primarily used by results that return lists.</param>
        /// <returns>A DataAccessResult&lt;T&gt; with the results of the GraphQl result from StrawberryShake</returns>
        private static DataAccessResult<T> GraphQLResult<T>(IOperationResult? graphQLResult, string subProperty = "")
            where T : new()
        {
            var resultData = graphQLResult?.Data;
            if (null == resultData)
                return new DataAccessResult<T>
                { Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
            if (string.IsNullOrWhiteSpace(subProperty))
            {
                var data = ThisToThat.ToEntityType<T>(graphQLResult?.Data);
                return new DataAccessResult<T>
                { Data = data, Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
            }
            //Get value of subproperty
            dynamic resultValue = resultData;
            var levels = subProperty.Split('.');
            foreach (var level in levels)
            {
                var subPropertyInfo = new ReflectionProperty(resultData.GetType(), level);
                var propInfo = subPropertyInfo.PropertyInfo();
                if (null == propInfo)
#pragma warning disable CA2201
                    throw new NullReferenceException($"{levels} {level}");
#pragma warning restore CA2201
                resultValue = subPropertyInfo.PropertyInfo().GetValue(resultValue);
            }
            var subData = ThisToThat.ToEntityType<T>(resultValue);
            return new DataAccessResult<T>
            { Data = subData, Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
        }
        private static SaveDataResult GraphQLSaveResult(IOperationResult? graphQLResult)
        {
            return new SaveDataResult
            { Errors = graphQLResult?.Errors.Select(ErrorToString).ToArray() ?? [] };
        }

        private async Task<IDataAccessResult<T>> ExecuteGet<T>(Func<Task<IOperationResult>> dataFunc,
            string subProperty = "",
            [CallerMemberName] string graphQlFunctionName = "GraphQL call", T? defaultValue = null) where T : class, new()
        {
            try
            {
                var result = await dataFunc();
                var gqlResult = GraphQLResult<T>(result, subProperty);
                return gqlResult;
            }
            catch (Exception ex)
            {
                var exceptionDetail = ex.ToText();
                logging.LogException(ex, graphQlFunctionName + " returned exception",
                    exceptionDetail, Severity.Error, "GraphQl");
                return new DataAccessResult<T> { Data = defaultValue, Errors = [ex.Message] };
            }
        }

        /// <summary>
        /// Central logic for properly handling any errors when saving data
        /// </summary>
        /// <param name="dataFunc">Save data operation</param>
        /// <param name="graphQlFunctionName">The GraphQl function name to include in exception message if an exception is thrown</param>
        /// <returns>An SaveDataResult that either confirms success or returns errors in standard format</returns>
        private async Task<ISaveDataResult> ExecuteSave(Func<Task<IOperationResult>> dataFunc,
            [CallerMemberName] string graphQlFunctionName = "GraphQL call")
        {
            try
            {
                await dataFunc();
                return new SaveDataResult();
            }
            catch (Exception ex)
            {
                var exceptionDetail = ex.ToText();
                logging.LogException(ex, graphQlFunctionName + " returned exception",
                    exceptionDetail, Severity.Error, "GraphQl");
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }
    }

    /// <summary>
    /// Class used to get PropertyInfo objects from data types that are cached after reflection returns them for the first time.
    /// </summary>
    /// <param name="type">Type that contains the property</param>
    /// <param name="propertyName">Name of the property to return the PropertyInfo for</param>
    internal sealed class ReflectionProperty(Type type, string propertyName)
    {
        private static readonly Dictionary<ReflectionProperty, PropertyInfo> _reflectedPropertyCache = [];
        private Type Type => type;
        private string PropertyName => propertyName;
        public override string ToString()
        {
            return $"{Type}.{PropertyName}";
        }

        public override int GetHashCode()
        {
            return (Type.GetHashCode() + PropertyName.GetHashCode()).GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }

        public PropertyInfo PropertyInfo()
        {
            if (PropertyName.Contains('.'))
                throw new NotSupportedException("Properties of properties is not currently supported");
            if (!_reflectedPropertyCache.ContainsKey(this))
            {
                var reflectedProp = Type.GetProperty(PropertyName);
                if (null == reflectedProp)
                    throw new MissingMemberException("Property does not exist on type.");
                _reflectedPropertyCache[this] = reflectedProp;
            }
            return _reflectedPropertyCache[this];
        }
    }
}
