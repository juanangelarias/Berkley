using HotChocolate.Subscriptions;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Shared.Data;

namespace James.Data.Server
{
    //TODO: Review if using this with injected classes causes any issues similar to GraphQl queries with injected classes
    public class ServerDataAccess(IDbContextFactory<JamesDatabaseContext> contextFactory, Query query, AgencyMutation agencyMutation, ITopicEventSender eventSender) : IDataAccess
    {
        public async Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber)
        {
            try
            {
                var result = await query.GetAgencyAccounts(agencyNumber, contextFactory);
                return new DataAccessResult<List<Account>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Account>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Account>> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<Agency>> GetAgencyByAgencyNumber(string agencyNumber)
        {
            try
            {
                var result = await query.GetAgencyByAgencyNumber(agencyNumber, contextFactory);
                return null == result
                    ? new DataAccessResult<Agency> {Errors = ["No agency With that agency number was found."]}
                    : new DataAccessResult<Agency> {Data = result};
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

        public async Task<IDataAccessResult<List<AgencyLicense>>> GetAgencyLicenses(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyLicenses(agencyId, contextFactory);
                return new DataAccessResult<List<AgencyLicense>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<AgencyLicense>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<AgencyLicense>> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<List<Insurer>>> GetAllInsurers()
        {
            try
            {
                var result = await query.GetAllInsurers(contextFactory);
                return new DataAccessResult<List<Insurer>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Insurer>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Insurer>> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<List<State>>> GetAllStates()
        {
            try
            {
                var result = await query.GetAllStates(contextFactory);
                return new DataAccessResult<List<State>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<State>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<State>> { Errors = [ex.Message] };
            }

        }

        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyBonds(agencyId, contextFactory);
                return new DataAccessResult<List<Bond>> { Data = result };
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

        public async Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyAgents(agencyId, contextFactory);
                return new DataAccessResult<List<AgentsInAgency>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<AgentsInAgency>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<AgentsInAgency>> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            try
            {
                var result = await query.GetAgencyStatuses(contextFactory);
                return new DataAccessResult<List<AgencyStatusDm>> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<AgencyStatusDm>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<AgencyStatusDm>> { Errors = [ex.Message] };
            }

        }

        //public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        //{
        //    try
        //    {
        //        var result = await query.GetAgencyPOAs(agencyId, contextFactory);
        //        return new DataAccessResult<List<PowerOfAttorney>> { Data = result };
        //    }
        //    catch (AggregateException ae)
        //    {
        //        return new DataAccessResult<List<PowerOfAttorney>> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new DataAccessResult<List<PowerOfAttorney>> { Errors = [ex.Message] };
        //    }
        //}

        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            try
            {
                var result = await  query.GetAgentByAgentId(agentId, contextFactory);
                return new DataAccessResult<Agent> { Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<Agent> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<Agent> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<List<Agency>>> GetAgencyRelatedParties(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyRelatedParties(agencyId, contextFactory);
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

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search)
        {
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
        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyPOAs(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<List<PowerOfAttorneyStatusDm>>> GetAllPoaStatuses()
        {
            return await ExecuteGet(async()=> await query.GetAllPoaStatuses(contextFactory));
        }

        public async Task<IDataAccessResult<List<AgencyCommission>>> GetAgencyCommissionRates(Guid agencyId)
        {
            return await ExecuteGet(async () => await query.GetAgencyCommissionRates(agencyId, contextFactory));
        }

        public async Task<IDataAccessResult<PowerOfAttorney>> SetPowerOfAttorney(Guid poaId, Guid insurerId, int? limit, string? serial, DateOnly? firstIssued,
            DateOnly? currentIssued, string? comments, Guid status)
        {
            return await ExecuteGet(async()=> await agencyMutation.SetPowerOfAttorney(poaId, insurerId, limit, serial, firstIssued, currentIssued, comments, status, eventSender, contextFactory));
        }

        public async Task<ISaveDataResult> SetAddress(Address address)
        {
            try
            {
                await agencyMutation.SetAddress(address.Id, address.Address1, address.Address2, address.Address3, address.City, address.StateCode, address.PostalCode,
                eventSender, contextFactory);
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

        public async Task<IDataAccessResult<AgencyLicense>> CreateLicense(Guid agencyId, Guid? agentId, bool? appointingState, string? comments, DateOnly? appointment, DateOnly? expiration, DateOnly? termination,
            Guid insurerId, bool isResident, string? licenseNumber, string state, bool isActive)
        {
            try
            {
                var result = await agencyMutation.CreateLicense(agencyId, agentId, appointingState, 
                    comments, appointment, expiration, termination,
                    insurerId, isResident, licenseNumber, state, isActive, eventSender, contextFactory);
                return new DataAccessResult<AgencyLicense>{Data = result };
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<AgencyLicense> { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new DataAccessResult<AgencyLicense> { Errors = [ex.Message] };
            }
        }

        public async Task<IDataAccessResult<AgencyLicense>> SetAgencyLicense(Guid licenseId, Guid agencyId, Guid? agentId, bool? appointingState, string? comments,
            DateOnly? appointment, DateOnly? expiration, DateOnly? termination, Guid insurerId, bool isResident,
            string? licenseNumber, string state, bool isActive)
        {
            //TODO:Test this before refactoring the rest
            return await ExecuteGet(async ()=> await agencyMutation.SetAgencyLicense(licenseId, agencyId, agentId, appointingState,
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
            //TODO:Test this before refactoring the rest
            return await ExecuteSave(async () => await agencyMutation.DeleteLicense(licenseId, eventSender, contextFactory));

            //try
            //{
            //    await agencyMutation.DeleteLicense(licenseId, eventSender, contextFactory);
            //    return new SaveDataResult();
            //}
            //catch (AggregateException ae)
            //{
            //    return new SaveDataResult { Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            //}
            //catch (Exception ex)
            //{
            //    return new SaveDataResult { Errors = [ex.Message] };
            //}
        }

        public async Task<ISaveDataResult> SetAgencyCommissionRates(Guid agencyId, AgencyCommission[] rates)
        {
            return await ExecuteSave(async ()=>await agencyMutation.SaveCommissionRates(agencyId, rates, eventSender, contextFactory));
        }

        //UNDONE:  Refactor to DRY out the code
        private async Task<IDataAccessResult<T>> ExecuteGet<T>( Func<Task<T>> dataFunc)
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
                return new SaveDataResult{ Errors = ae.InnerExceptions.Select(e => e.Message).ToArray() };
            }
            catch (Exception ex)
            {
                return new SaveDataResult{ Errors = [ex.Message] };
            }
        }

        public async Task<IDisposable> AddressModified()
        {
            //UNDONE:
            return await Task.FromResult( FakeSubscription.Create);
        }
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
