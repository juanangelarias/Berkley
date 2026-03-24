using HotChocolate.Authorization;

namespace James.Data.Server.GraphQL.Mutations;

[MutationType]
public partial class GeneralMutation
{
    [Authorize]
    public async Task<bool> SetAgent(Guid agentAgencyId, Guid agentId, string givenName, string middleInitial,
        string familyName, string nationalProducerNumber, string countryCode, string phoneNumber, string email,
        string? extension, Guid agencyId, bool aif, bool isNewAgent,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        try
        {
            // 1. Handle Agent
            var agent = await ctx.Agents
                .FirstOrDefaultAsync(a => a.Id == agentId);

            if (agent == null)
            {
                if (!isNewAgent)
                    throw new GraphQLException($"Agent with Id: {agentId} was not found.");

                agent = new() { Id = agentId };

                ctx.Agents.Add(agent);
            }

            agent.NationalProducerNumber = nationalProducerNumber;

            // 2. Handle Legal Entity
            var legalEntity = await ctx.LegalEntities
                .FirstOrDefaultAsync(a => a.Id == agentId);

            if (legalEntity == null)
            {
                if (!isNewAgent)
                    throw new GraphQLException($"LegalEntity with Id: {agentId} was not found.");

                legalEntity = new LegalEntity
                {
                    Id = agentId,
                    IsIndividual = true,
                    EntityType = "Agent",
                    Parent = agentId
                };

                ctx.LegalEntities.Add(legalEntity);
            }

            legalEntity.FullName = string.IsNullOrWhiteSpace(middleInitial)
                ? $"{givenName} {familyName}"
                : $"{givenName} {middleInitial} {familyName}";
            legalEntity.GivenName = givenName;
            legalEntity.MiddleInitial = middleInitial;
            legalEntity.FamilyName = familyName;

            // 3. Handle Email
            var emailEntity = await ctx.LegalEntityEmails
                .FirstOrDefaultAsync(e => e.LegalEntityId == agentId && e.Type == "Main");
            if (emailEntity == null)
            {
                emailEntity = new LegalEntityEmail
                {
                    Id = Guid.NewGuid(),
                    LegalEntityId = agentId,
                    Type = "Main"
                };
                ctx.LegalEntityEmails.Add(emailEntity);
            }

            emailEntity.EmailAddress = email;

            // 4. Handle Phone
            var lePhone = await ctx.LegalEntityPhones
                .Include(p => p.PhoneNumber)
                .FirstOrDefaultAsync(p => p.LegalEntityId == agentId && p.Type == "Main");

            if (lePhone == null)
            {
                var phone = new PhoneNumber { Id = Guid.NewGuid() };
                ctx.PhoneNumbers.Add(phone);

                lePhone = new LegalEntityPhone
                {
                    LegalEntityId = agentId,
                    PhoneNumberId = phone.Id,
                    Type = "Main",
                    PhoneNumber = phone
                };
                ctx.LegalEntityPhones.Add(lePhone);
            }

            lePhone.PhoneNumber.CountryCode = countryCode;
            lePhone.PhoneNumber.MainNumber = phoneNumber;
            lePhone.PhoneNumber.Extension = extension;

            // 5. Handle Agency Link
            var agencyAgentLink = await ctx.AgentsInAgencies
                .FirstOrDefaultAsync(aa => aa.Id == agentAgencyId);

            if (agencyAgentLink == null)
            {
                agencyAgentLink = new AgentsInAgency
                {
                    Id = agentAgencyId,
                    AgencyId = agencyId,
                    AgentId = agentId,
                    Active = true,
                    PortalUser = false
                };
                ctx.AgentsInAgencies.Add(agencyAgentLink);
            }

            agencyAgentLink.AttorneyInFact = aif;

            await ctx.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (GraphQLException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new GraphQLException($"Error when processing agent: {ex.Message}", ex);
        }
    }

    [Authorize]
    public async Task<bool> TransferAgent(Guid agentId, Guid originAgencyId, Guid destinationAgencyId,
        bool transferLicenses, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        try
        {
            var agencyAgentOrigin = await ctx.AgentsInAgencies
                .FirstOrDefaultAsync(aa => aa.AgentId == agentId && aa.AgencyId == originAgencyId);

            if (agencyAgentOrigin == null)
                throw new GraphQLException($"Agent with Id: {agentId} was not found in Agency: {originAgencyId}");

            agencyAgentOrigin.Active = false;

            var agencyAgentDestination = new AgentsInAgency
            {
                Id = Guid.NewGuid(),
                AgencyId = destinationAgencyId,
                AgentId = agentId,
                Active = true,
                PortalUser = false,
                AttorneyInFact = agencyAgentOrigin.AttorneyInFact
            };

            ctx.AgentsInAgencies.Add(agencyAgentDestination);

            var licenses = ctx.AgencyLicenses
                .Where(l => l.AgentId == agentId && l.AgencyId == originAgencyId)
                .ToList();

            licenses.ForEach(l => l.IsActive = false);
            ctx.AgencyLicenses.UpdateRange(licenses);

            if (transferLicenses)
            {
                var destinationLicenses = licenses
                    .Select(l => new AgencyLicense
                    {
                        Id = Guid.NewGuid(),
                        AgencyId = destinationAgencyId,
                        AgentId = agentId,
                        State = l.State,
                        LicenseNumber = l.LicenseNumber,
                        IsResident = l.IsResident,
                        InsurerId = l.InsurerId,
                        Expiration = l.Expiration,
                        Comments = l.Comments,
                        Appointment = l.Appointment,
                        Termination = l.Termination,
                        AppointingState = l.AppointingState,
                        IsActive = true,
                        ImagingId = l.ImagingId
                    })
                    .ToList();

                await ctx.AgencyLicenses.AddRangeAsync(destinationLicenses);
            }

            await ctx.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (GraphQLException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            throw new GraphQLException($"Error when transferring agent: {exception.Message}", exception);
        }
    }

    [Authorize]
    public async Task<bool> UpdateAndTransferAgent(Guid agentAgencyId, Guid agentId, string givenName, string middleInitial,
        string familyName, string nationalProducerNumber, string countryCode, string phoneNumber, string email,
        string? extension, bool aif, Guid originAgencyId, Guid destinationAgencyId, 
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        // Update Agent data
        var response = await SetAgent(agentAgencyId, agentId, givenName, middleInitial, familyName, nationalProducerNumber,
            countryCode, phoneNumber, email, extension, destinationAgencyId, aif, false, contextFactory);
        if(!response)
            throw new GraphQLException("Error when updating agent data");
        
        // Transfer Agent
        response = await TransferAgent(agentId, originAgencyId, destinationAgencyId, true, contextFactory);
        return !response 
            ? throw new GraphQLException("Error when transferring agent") 
            : true;
    }
    
    [Authorize]
    public async Task<bool> AssignAgent(Guid agentId, Guid agencyId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();
        
        await using var transaction = await ctx.Database.BeginTransactionAsync();

        try
        {
            var agentInAgency = new AgentsInAgency
            {
                Id = Guid.NewGuid(),
                AgencyId = agencyId,
                AgentId = agentId,
                Active = true,
                AttorneyInFact = false,
                PortalUser = false
            };
            
            ctx.AgentsInAgencies.Add(agentInAgency);
            
            await ctx.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return true;
        }
        catch (GraphQLException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            throw new GraphQLException($"Error when transferring agent: {exception.Message}", exception);
        }
    }

    [Authorize]
    public async Task<bool> DisassociateAgent(Guid agentId, Guid agencyId,
        [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
    {
        var ctx = await contextFactory.CreateDbContextAsync();

        await using var transaction = await ctx.Database.BeginTransactionAsync();

        try
        {
            var agencyAgent = await ctx.AgentsInAgencies
                .FirstOrDefaultAsync(aa => aa.AgentId == agentId && aa.AgencyId == agencyId);

            if (agencyAgent == null)
                throw new GraphQLException($"Agent with Id: {agentId} was not found in Agency: {agencyId}");

            agencyAgent.Active = false;

            var licenses = ctx.AgencyLicenses
                .Where(l => l.AgentId == agentId && l.AgencyId == agencyId)
                .ToList();

            licenses.ForEach(l => l.IsActive = false);

            ctx.AgentsInAgencies.Update(agencyAgent);
            ctx.AgencyLicenses.UpdateRange(licenses);
            await ctx.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (GraphQLException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            throw new GraphQLException($"Error when transferring agent: {exception.Message}", exception);
        }
    }
}