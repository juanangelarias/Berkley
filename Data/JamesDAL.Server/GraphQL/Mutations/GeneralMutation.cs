using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using James.Shared;
using James.Shared.Data;
using James.Shared.Server;

namespace James.Data.Server.GraphQL.Mutations
{
    [MutationType]
    public class GeneralMutation
    {
        [Authorize]
        public async Task<bool> CreateAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode,
            Guid legalEntityId, string addressType, string identifier,
            [Service] ITopicEventSender eventSender,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                Address NewAddress = new Address()
                {
                    Id = addressId,
                    Address1 = address1,
                    Address2 = address2,
                    Address3 = address3,
                    City = city,
                    StateCode = stateCode,
                    PostalCode = postalCode
                };

                var ctx = await contextFactory.CreateDbContextAsync();

                ctx.Addresses.Add(NewAddress);

                var newLEAddress = new LegalEntityAddress
                {
                    LegalEntityId = legalEntityId,
                    AddressId = addressId,
                    Type = addressType
                };

                ctx.LegalEntityAddresses.Add(newLEAddress);
                await ctx.SaveChangesAsync();
                eventSender.SendAsync($"{nameof(Subscription.OnAddressCollectionModified)}_{legalEntityId}",
                    new SubscriptionResult<string>() { Identifier = identifier, Result = addressId.ToString() });

                return true;
            }
            catch (Exception ex)
            {

                loggingService.LogException(ex, "Exception saving created address to database", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }
        
        [Authorize]
        public async Task<bool> CreatePhoneNumber(Guid phoneId, string? countryCode, string mainNumber, string? extension,
            Guid legalEntityId, string phoneType, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                PhoneNumber newNumber = new PhoneNumber()
                {
                    Id = phoneId,
                    CountryCode = countryCode ?? "US",
                    MainNumber = mainNumber,
                    Extension = extension
                };

                LegalEntityPhone lePhone = new LegalEntityPhone()
                {
                    LegalEntityId = legalEntityId,
                    PhoneNumberId = phoneId,
                    Type = phoneType
                };

                var ctx = await contextFactory.CreateDbContextAsync();

                ctx.PhoneNumbers.Add(newNumber);
                ctx.LegalEntityPhones.Add(lePhone);
                await ctx.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [Authorize]
        public async Task<bool> DeleteAddress(Guid addressId, string identifier,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var address = await ctx.Addresses.SingleOrDefaultAsync(x => x.Id == addressId);
                var leAddress = await ctx.LegalEntityAddresses.SingleOrDefaultAsync(x => x.AddressId == addressId);

                if (leAddress != null)
                {
                    ctx.LegalEntityAddresses.Remove(leAddress);
                }
                if (address != null)
                {
                    ctx.Addresses.Remove(address);
                }

                await ctx.SaveChangesAsync();
                await eventSender.SendAsync($"{nameof(Subscription.OnAddressCollectionModified)}_{leAddress?.LegalEntityId}",
                    new SubscriptionResult<string>() { Identifier = identifier, Result = identifier });
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception deleting address to database", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }

        [Authorize]
        public async Task<Address> SetAddress(Guid addressId, string address1, string? address2,
            string? address3, string city, string? stateCode, string? postalCode, string identifier,
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
                loggingService.LogException(ex, "Exception saving address to database", category: StandardLoggingCategories.DataAccess);
            }

            await eventSender.SendAsync($"{nameof(Subscription.OnAddressModified)}_{addressId}", new SubscriptionResult<Address> { Identifier = identifier, Result = oldAddress });

            return oldAddress;
        }
        
        [Authorize]
        public async Task<bool> DeletePhoneNumber(Guid phoneId, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();

                var phone = await ctx.PhoneNumbers.SingleOrDefaultAsync(x => x.Id == phoneId);
                var lePhone = await ctx.LegalEntityPhones.SingleOrDefaultAsync(x => x.PhoneNumberId == phoneId);

                if (lePhone != null)
                {
                    ctx.LegalEntityPhones.Remove(lePhone);
                }
                if (phone != null)
                {
                    ctx.PhoneNumbers.Remove(phone);
                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Authorize]
        public async Task<LegalEntityEmail> SetLegalEntityEmail(Guid id, Guid legalEntityId, string emailAddress,
            string type, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();

            var oldRecord = await ctx.LegalEntityEmails
                .FirstOrDefaultAsync(r => r.Id == id);

            if (oldRecord == null)
            {
                var newRecord = new LegalEntityEmail
                {
                    Id = id,
                    LegalEntityId = legalEntityId,
                    EmailAddress = emailAddress,
                    Type = type,
                    Created = DateTime.Today,
                    Modified = DateTime.Today
                };
                ctx.LegalEntityEmails.Add(newRecord);
                await ctx.SaveChangesAsync();
                
                return newRecord;
            }

            oldRecord.EmailAddress = emailAddress;
            oldRecord.Type = type;
            oldRecord.Modified = DateTime.Today;
            ctx.Update(oldRecord);
            await ctx.SaveChangesAsync();
            
            return oldRecord;
        }

        [Authorize]
        public async Task<bool> DeleteLegalEntityEmail(Guid id,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var oldRecord = await ctx.LegalEntityEmails
                .FirstOrDefaultAsync(r => r.Id == id);
            if (oldRecord == null)
                return false;
            
            ctx.LegalEntityEmails.Remove(oldRecord);
            await ctx.SaveChangesAsync();
            
            return true;
        }

        [Authorize]
        public async Task<bool> SetUserSetting(string key, string? value, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] IUserShared userShared, [Service] ILoggingService loggingService)
        {
            var username = (await userShared.GetCurrentUser()).Username;
            if (null == username)
                throw new UnauthorizedAccessException("You must be logged in to set user settings.");
            return await SetUserSetting(key, value, username, contextFactory, loggingService);
        }

        [Authorize]
        public async Task<bool> SetDefaultUserSetting(string key, string? value, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            return await SetUserSetting(key, value, "Default", contextFactory, loggingService);
        }

        private async Task<bool> SetUserSetting(string key, string? value, string username, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                //TODO:Change to UserSettings below when schema change is done
                var existing = await ctx.UserPreferences.FirstOrDefaultAsync(up => up.Username == username && up.Key == key);
                if (existing == null)
                {
                    if (value == null)
                        return true;//Nothing to delete from DB
                    //TODO:Change to UserSettings below when schema change is done
                    ctx.UserPreferences.Add(new UserPreference { Username = username, Key = key, Value = value });
                    await ctx.SaveChangesAsync();
                }
                else
                {
                    if (value == null)
                    {
                        //TODO:Change to UserSettings below when schema change is done
                        ctx.UserPreferences.Remove(existing);
                        await ctx.SaveChangesAsync();
                        return true;
                    }
                    existing.Value = value;
                    //TODO:Change to UserSettings below when schema change is done
                    ctx.UserPreferences.Update(existing);
                }
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception saving user setting to database", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }

        //TODO: Restrict to people in the change permissions role
        [Authorize]
        public async Task<bool> AddPrincipalToSecurityRole(Guid principalId, string role,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var existing =
                    await ctx.Securities.FirstOrDefaultAsync(s => s.Role == role && s.PrincipalId == principalId);
                if (null == existing)
                {
                    await ctx.Securities.AddAsync(new Security { PrincipalId = principalId, Role = role });
                    await ctx.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception adding principal to security role", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }

        //TODO: Restrict to people in the change permissions role
        [Authorize]
        public async Task<bool> RemovePrincipalFromSecurityRole(Guid principalId, string role,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            try
            {
                var ctx = await contextFactory.CreateDbContextAsync();
                var existing =
                    await ctx.Securities.FirstOrDefaultAsync(s => s.Role == role && s.PrincipalId == principalId);
                if (null == existing) return true;
                ctx.Securities.Remove(existing);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception removing principal from security role", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }

        //TODO: Restrict to people in the change permissions role
        [Authorize]
        public async Task<bool> AddSecurityRole(string role, string description, int ord,
            [Service] IDbContextFactory<JamesDatabaseContext> contextFactory, [Service] ILoggingService loggingService)
        {
            //TODO:Update to fire a subscription event if the client caches the full list
            try
            {
                if (string.IsNullOrWhiteSpace(role))
                    throw new ArgumentException("role.Role cannot be null or whitespace.");
                var newRole = new SecurityRole
                {
                    Id = Guid.NewGuid(), Role = role, Description = description, Ord = ord
                };
                var ctx = await contextFactory.CreateDbContextAsync();
                var existing =
                    await ctx.SecurityRoles.FirstOrDefaultAsync(s => s.Role == newRole.Role);
                if (null != existing)
                    return false;
                if (newRole.Ord < 1)
                {
                    //Set the order to one higher than the previous max.
                    //HACK: Not worrying about a transaction to prevent simultaneously creating roles that might have the same order
                    var maxOrd = await ctx.SecurityRoles.MaxAsync(sr => sr.Ord);
                    newRole.Ord = 1 + maxOrd;
                }
                await ctx.SecurityRoles.AddAsync(newRole);
                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                loggingService.LogException(ex, "Exception adding security role", category: StandardLoggingCategories.DataAccess);
                return false;
            }
        }

    }
}
