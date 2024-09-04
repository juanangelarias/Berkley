using HotChocolate.Subscriptions;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Shared;
using James.Shared.Data;
using System.Diagnostics.Contracts;

namespace James.Data.Server
{
    //TODO: Review if using this with injected classes causes any issues similar to GraphQl queries with injected classes
    public class ServerDataAccess(IDbContextFactory<JamesDatabaseContext> contextFactory, Query query, AgencyMutation agencyMutation, ObligeeMutation obligeeMutation, GeneralMutations generalMutations, ITopicEventSender eventSender, ITopicEventReceiver eventReceiver, ILoggingService loggingService) : IDataAccess
    {
        public async Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber)
        {
            return await ExecuteGet(async () => await query.GetAgencyAccounts(agencyNumber, contextFactory));
        }

        public async Task<IDataAccessResult<Agency>> GetAgencyByAgencyNumber(string agencyNumber)
        {
            //TODO: Port this to ExecuteGet
            try
            {
                var result = await query.GetAgencyByAgencyNumber(agencyNumber, contextFactory);
                return null == result
                    ? new DataAccessResult<Agency> { Errors = ["No agency With that agency number was found."] }
                    : new DataAccessResult<Agency> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<Agency> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<Agency> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<List<Obligee>>> SearchObligees(string searchString)
        {
            try
            {
                var result = await query.SearchObligeesAsync(searchString, contextFactory);
                return null == result
                    ? new DataAccessResult<List<Obligee>> { Errors = ["No obligees were found."] }
                    : new DataAccessResult<List<Obligee>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Obligee>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Obligee>> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<Obligee?>> GetObligeeById(Guid id)
        {
            try
            {
                var result = await query.GetObligeeById(id, contextFactory);
                return null == result
                    ? new DataAccessResult<Obligee?> { Errors = ["No obligee with the supplied id was found."] }
                    : new DataAccessResult<Obligee?> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<Obligee?> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<Obligee?> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<List<Bond>>> GetObligeePrimaryBonds(Guid obligeeId)
        {
            try 
            {
                var result = await query.GetObligeePrimaryBonds(obligeeId, contextFactory);
                return null == result
                    ? new DataAccessResult<List<Bond>> { Errors = ["No primary bonds were found for supplied ObligeeID"] }
                    : new DataAccessResult<List<Bond>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Bond>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Bond>> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<List<Bond>>> GetObligeeSecondaryBonds(Guid obligeeId)
        {
            try
            {
                var result = await query.GetObligeeSecondaryBonds(obligeeId, contextFactory);
                return null == result
                    ? new DataAccessResult<List<Bond>> { Errors = ["No secondary bonds were found for supplied ObligeeID"] }
                    : new DataAccessResult<List<Bond>> { Data = result };

            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Bond>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Bond>> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<List<AgencyInventory>>> GetAgencyInventory(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyInventory(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyLicenses(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            return await ExecuteGet(async () => await query.GetAllInsurers(contextFactory));
        }
        public async Task<IDataAccessResult<List<Branch>>> GetAllBranches()
        {
            return await ExecuteGet(async () => await query.GetAllBranches(contextFactory));
        }
        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            return await ExecuteGet(async () => await query.GetAllStates(contextFactory));
        }
        public async Task<IDataAccessResult<List<InventoryDocumentDm>>> GetAllInventoryDocTypes()
        {
            return await ExecuteGet(async () => await query.GetAllInventoryDocTypes(contextFactory));
        }
        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyBonds(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyAgents(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            return await ExecuteGet(async () => await query.GetAgencyStatuses(contextFactory));
        }
        public async Task<IDataAccessResult<List<AgencyStatusLog>>> GetAgencyStatusLog(string agencyNumber)
        {
            return await ExecuteGet(async () => await query.GetAgencyStatusLog(agencyNumber, contextFactory));
        }
        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            return await ExecuteGet(async () => await query.GetAgentByAgentId(agentId, contextFactory));
        }

        public async Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyRelatedParties(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search)
        {
            //TODO: Port to Execute Get
            try
            {
                var result = await query.SearchAgencies(search, contextFactory);
                return new DataAccessResult<List<Agency>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Agency>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Agency>> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<Address>> GetAddress(Guid addressId)
        {
            return await ExecuteGet(async () => await query.GetAddress(addressId, contextFactory));
        }
        public async Task<IDataAccessResult<List<AddressTypeDm>>> GetAddressTypes()
        {
            return await ExecuteGet(async () => await query.GetAddressTypes(contextFactory));
        }
        public async Task<IDataAccessResult<List<Address>>> GetAllLegalEntityAddresses(Guid legalEntityId)
        {
            return await ExecuteGet(async () => await query.GetAllLegalEntityAddresses(legalEntityId, contextFactory));
            
        }
        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyPOAs(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses()
        {
            return await ExecuteGet(async () => await query.GetAllPoaStatuses(contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyCommissionRates(agencyId, contextFactory));
        }
        public async Task<IDataAccessResult<UserProfile>> GetUserProfileByUserName(string userName)
        {
            return await ExecuteGet(async () => await query.GetUserProfileByUserName(userName, contextFactory));
        }
        public async Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? serial, DateOnly? firstIssued,
            DateOnly? currentIssued, string? comments, Guid status)
        {
            return await ExecuteGet(async () => await agencyMutation.SetPowerOfAttorney(poaId, insurerId, limit, serial, firstIssued, currentIssued, comments, status, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetAddress(Address address, string identifier)
        {
            return await ExecuteSave((async () => await agencyMutation.SetAddress(address.Id, address.Address1, address.Address2,
                address.Address3, address.City, address.StateCode, address.PostalCode, identifier,
                eventSender, contextFactory, loggingService)));
        }
        public async Task<ISaveDataResult> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType)
        {
            //TODO: Error Handling?
            return await ExecuteSave(async () =>
                await generalMutations.CreateAddress(addressId, address1, address2, address3, city, stateCode, postalCode, 
                legalEntityId, addressType, contextFactory));

        }
        public async Task<ISaveDataResult> DeleteAddress(Guid addressId)
        {
            //TODO: Implement
            return await ExecuteSave(async () =>
            await generalMutations.DeleteAddress(addressId, contextFactory));
        }
        public async Task<ISaveDataResult> SetAgencyGeneralInfo(Guid agencyId, string agencyName, Guid parentId, string? taxId, string? npn, bool w9,
            bool need1099, bool nasbp, string branchKey)
        {
            try
            {
                var result = await agencyMutation.SetAgencyGeneralInfo(agencyId, agencyName, parentId, taxId, npn, w9, need1099, nasbp, branchKey, contextFactory);
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }
        public async Task<ISaveDataResult> SetAgencyInventory(Guid inventoryId, DateTime? sent, int? quantity, string documentType, string? addressee,
            Guid addressId, string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode)
        {
            try
            {
                await agencyMutation.SetAgencyInventory(inventoryId, sent, quantity, documentType, addressee, addressId, address1, address2, address3, city, stateCode, postalCode,
                    eventSender, contextFactory, loggingService);
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }
        public async Task<ISaveDataResult> CreateAgencyStatusLog(Guid id, string agencyNumber, DateTime effective, string oldStatus, string newStatus, Guid changedBy, string? comments)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyStatusLog(id, agencyNumber, effective, oldStatus, newStatus, changedBy, comments, eventSender, contextFactory);
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }
        public async Task<ISaveDataResult> CreateAgencyPOA(Guid poaId, Guid insurerId, Guid agencyId, int limit, string? serial, DateOnly? firstIssued, DateOnly? currentIssued, string? comments, Guid status)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyPOA(poaId, insurerId, agencyId, limit, serial, firstIssued, currentIssued, comments, status, eventSender, contextFactory);
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }
        public async Task<ISaveDataResult> DeleteAgencyPOA(Guid poaId)
        {
            try
            {
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }
        public async Task<ISaveDataResult> CreateLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
        Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive)
        {
            try
            {
                var result = await agencyMutation.CreateLicense(licenseId, agencyId, agentId, appointingState,
                    comments, appointment, expiration, termination,
                    insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory, loggingService);
                return new DataAccessResult<bool> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<bool> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<bool> { Errors = [ex.Message] };
            }
        }
        public async Task<ISaveDataResult> CreateAgencyInventory(Guid inventoryId, Guid agencyId, DateTime dateSent, int quantity, string documentType, string addressee,
            string address1, string? address2, string? address3, string city, string? stateCode, string? postalCode, Guid approverId)
        {
            try
            {
                var result = await agencyMutation.CreateAgencyInventory(inventoryId, agencyId, dateSent, quantity, documentType, addressee,
                    address1, address2, address3, city, stateCode, postalCode, approverId, eventSender, contextFactory);
                return new DataAccessResult<bool>(); /*{  Data = result };*/
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<bool> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<bool> { Errors = [ex.Message] };
            }
        }

        public async Task<ISaveDataResult> CreateObligee(Guid id, string fullName, string obligeeType, bool printStatusLetter, string? notes,
            string address1, string? address2, string city, string state, string postalCode, string? phoneNumber, string? email)
        {
            try
            {
                var result = await obligeeMutation.CreateObligee(id, fullName, obligeeType, printStatusLetter, notes,
                    address1, address2, city, state, postalCode, phoneNumber, email, eventSender, contextFactory);
                return new DataAccessResult<bool>() { Data = true };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<bool> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<bool> { Errors = [ex.Message] };
            }
        }
        public async Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, string? comments,
            DateOnly? appointment, DateOnly? expiration, DateOnly? termination, Guid insurerId, bool isResident,
            string? licenseNumber, string state, bool isActive)
        {
            return await ExecuteGet(async () => await agencyMutation.SetAgencyLicense(licenseId, agencyId, agentId, appointingState,
                    comments, appointment, expiration, termination,
                    insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory));

            //try
            //{
            //    var result = await agencyMutation.SetAgencyLicense(licenseId,agencyId, agentId, appointingState,
            //        comments, appointment, expiration, termination,
            //        insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory);
            //    return new DataAccessResult<AgencyLicense> { Data = result };
            //}
            //catch (AggregateException ae)
            //{
            //    return new DataAccessResult<AgencyLicense> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            //}
            //catch (Exception ex)
            //{
            //    return new DataAccessResult<AgencyLicense> { Errors = [ex.Message] };
            //}
        }

        public async Task<ISaveDataResult> DeleteLicense(Guid licenseId)
        {
            return await ExecuteSave(async () => await agencyMutation.DeleteLicense(licenseId, eventSender, contextFactory));

        }
        public async Task<ISaveDataResult> DeleteAgencyInventory(Guid inventoryId)
        {
            return await ExecuteSave(async () => await agencyMutation.DeleteAgencyInventory(inventoryId, eventSender, contextFactory));
        }
        public async Task<ISaveDataResult> SetAgencyCommissionRates(Guid agencyId, AgencyCommission[] rates)
        {
            return await ExecuteSave(async () => await agencyMutation.SaveCommissionRates(agencyId, rates, eventSender, contextFactory));
        }

        //UNDONE:  Refactor to DRY out the code
        private async Task<IDataAccessResult<T>> ExecuteGet<T>(Func<Task<T>> dataFunc)
        {
            try
            {
                var result = await dataFunc();
                return new DataAccessResult<T> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<T> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<T> { Errors = [ex.Message] };
            }
        }
        private static async Task<ISaveDataResult> ExecuteSave(Func<Task> saveAction)
        {
            try
            {
                await saveAction();
                return new SaveDataResult();
            }
            catch (AggregateException ae)
            {
                return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult { Errors = [ex.Message] };
            }
        }

        public IDisposable AddressModified(Guid addressId, Action<SubscriptionResult<Address>> onNext, Action<Exception>? onError = null, Action? onComplete = null)
        {
            //var eventValueTask =
            //    await eventReceiver.SubscribeAsync<SubscriptionResult<Address>>("OnAddressModified");
            //eventValueTask.ReadEventsAsync();
            ////UNDONE:
            //return await Task.FromResult(FakeSubscription.Create);
            return OnAddressModified(addressId).Subscribe(new ServerSideSubscriptionSubscriber<SubscriptionResult<Address>>(onNext, onError, onComplete));
        }

        private readonly Dictionary<Guid, ServerSideSubscription<SubscriptionResult<Address>>> _onAddressModified = new();

        private ServerSideSubscription<SubscriptionResult<Address>> OnAddressModified(Guid addressId)
        {
            lock (_onAddressModified)
            {
                if (_onAddressModified.ContainsKey(addressId) == false)
                {
                    _onAddressModified[addressId] = new(eventReceiver
                        .SubscribeAsync<SubscriptionResult<Address>>("OnAddressModified_"+addressId).Result.ReadEventsAsync(), CancellationToken.None);
                }

                return _onAddressModified[addressId];
            }
        }
        //public async Task<IDisposable> AddressModified(CancellationToken cancellationToken = default)
        //{
        //    var eventValueTask =
        //        await eventReceiver.SubscribeAsync<SubscriptionResult<Address>>("OnAddressModified", cancellationToken);
        //    eventValueTask.ReadEventsAsync();
        //    //UNDONE:
        //    return await Task.FromResult(FakeSubscription.Create);
        //}
    }
    //TODO:Remove when subscriptions are handled
    public class FakeSubscription : IDisposable
    {
        public static FakeSubscription Create => new();
        public void Dispose()
        {
            //Just a fake object.;
        }
    }
}
