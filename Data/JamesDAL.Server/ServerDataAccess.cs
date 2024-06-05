using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Subscriptions;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Data.Server.Model;
using James.Shared.Data;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace James.Data.Server
{
    //TODO: Review if using this with injected classes causes any issues similar to GraphQl queries with injected classes
    public class ServerDataAccess(IDbContextFactory<JamesDatabaseContext> contextFactory, Query query, AgencyMutation agencyMutation, ITopicEventSender eventSender) : IDataAccess
    {
        public async Task<IDataAccessResult<List<Account>>> GetAgencyAccounts(string agencyNumber)
        {
            try
            {
                var result = query.GetAgencyAccounts(agencyNumber, contextFactory);
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
                var result = query.GetAgencyByAgencyNumber(agencyNumber, contextFactory);
                if (null == result)
                    return new DataAccessResult<Agency> { Errors = ["No agency With that agency number was found."] };
                return new DataAccessResult<Agency> { Data = result };
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
                return new DataAccessResult<List<State>> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<State>> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<State>> {Errors = [ex.Message]};
            }

        }

        public async Task<IDataAccessResult<List<Bond>>> GetAgencyBonds(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyBonds(agencyId, contextFactory);
                return new DataAccessResult<List<Bond>> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Bond>> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Bond>> {Errors = [ex.Message]};
            }

        }

        public async Task<IDataAccessResult<List<AgentsInAgency>>> GetAgencyAgents(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyAgents(agencyId,contextFactory);
                return new DataAccessResult<List<AgentsInAgency>> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<AgentsInAgency>> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<AgentsInAgency>> {Errors = [ex.Message]};
            }
        }

        public async Task<IDataAccessResult<List<AgencyStatusDm>>> GetAgencyStatuses()
        {
            try
            {
                var result = await query.GetAgencyStatuses( contextFactory);
                return new DataAccessResult<List<AgencyStatusDm>> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<AgencyStatusDm>> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<AgencyStatusDm>> {Errors = [ex.Message]};
            }

        }

        public async Task<IDataAccessResult<List<PowerOfAttorney>>> GetAgencyPoas(Guid agencyId)
        {
            try
            {
                var result = await query.GetAgencyPOAs(agencyId, contextFactory);
                return new DataAccessResult<List<PowerOfAttorney>> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<PowerOfAttorney>> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<PowerOfAttorney>> {Errors = [ex.Message]};
            }
        }

        public async Task<IDataAccessResult<Agent>> GetAgent(Guid agentId)
        {
            try
            {
                var result = query.GetAgentByAgentId(agentId, contextFactory);
                return new DataAccessResult<Agent> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<Agent> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<Agent> {Errors = [ex.Message]};
            }
        }

        public async Task<IDataAccessResult<List<Agency>>> SearchAgencies(string? search)
        {
            try
            {
                var result = await query.SearchAgencies(search, contextFactory);
                return new DataAccessResult<List<Agency>> {Data = result};
            }
            catch (AggregateException ae)
            {
                return new DataAccessResult<List<Agency>> {Errors = ae.InnerExceptions.Select(e => e.Message).ToArray()};
            }
            catch (Exception ex)
            {
                return new DataAccessResult<List<Agency>> {Errors = [ex.Message]};
            }

        }

        public async Task SetAddress(Address address)
        {
            await agencyMutation.SetAddress(new SetAddressInput { AddressId = address.Id, Address1 = address.Address1, Address2 = address.Address2, Address3 = address.Address3, City = address.City, StateCode = address.StateCode, PostalCode = address.PostalCode },
                eventSender, contextFactory);
        }

        public async Task<IDisposable> AddressModified()
        {
            //UNDONE:
            return FakeSubscription.Create;
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
