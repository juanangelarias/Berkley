using System.Runtime.CompilerServices;
using HotChocolate.Data;
using HotChocolate.Execution.Configuration;
using HotChocolate.Subscriptions;
using James.Data.Server.GraphQL;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Data.Server.Model;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;
using James.Shared.Server;
using JamesWebUI.Server.SharedServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;

namespace James.Data.Server.Test
{
    public class SubscriptionTests(ITestOutputHelper output)
    {
        [Fact]
        public async Task OnAddressModified()
        {
            await using var services = CreateServer();
            var dataAccess = (IDataAccess)services.GetService(typeof(IDataAccess))!;
            var didSubscriptionFire = false;
            var subscriptionFireCount = 0;
            SubscriptionResult<Address>? subscriptionPayload = null;
            var unusedAddresses = await GetUnusedAddresses(services);
            var unusedAddress = unusedAddresses.FirstOrDefault();
            if (null == unusedAddress) return;
            var subscriptionAction = new Action<SubscriptionResult<Address>>(sr =>
            {
                didSubscriptionFire = true;
                Interlocked.Increment(ref subscriptionFireCount);
                subscriptionPayload = sr;
            });
            var origAddress3 = await ChangeAndChangeBack(dataAccess, unusedAddress, subscriptionAction);

            //Make sure subscription is fired
            Assert.True(didSubscriptionFire);
            if (subscriptionFireCount < 2)
                await Task.Delay(10); //Give 2nd subscription time to fire, if needed
            Assert.Equal(2, subscriptionFireCount);

            Assert.NotNull(subscriptionPayload);

            Assert.Equal(unusedAddress.Id, subscriptionPayload.Result.Id);
            Assert.Equal(origAddress3, subscriptionPayload.Result.Address3);
        }

        [Fact]
        public async Task OnAddressModifiedTwoChannels()
        {
            await using var services = CreateServer();
            var dataAccess = (IDataAccess)services.GetService(typeof(IDataAccess))!;
            var didSubscription1Fire = false;
            var subscriptionFireCount = 0;
            SubscriptionResult<Address>? subscription1Payload = null;
            var didSubscription2Fire = false;
            SubscriptionResult<Address>? subscription2Payload = null;


            var unusedAddresses = await GetUnusedAddresses(services);
            var unusedAddress1 = unusedAddresses.FirstOrDefault();
            if (null == unusedAddress1) return;
            var unusedAddress2 = unusedAddresses.LastOrDefault();
            if (null == unusedAddress2) return;
            //Subscribe
            var subscription1Action = new Action<SubscriptionResult<Address>>(sr =>
            {
                didSubscription1Fire = true;
                Interlocked.Increment(ref subscriptionFireCount);
                subscription1Payload = sr;
            });
            var subscription2Action = new Action<SubscriptionResult<Address>>(sr =>
            {
                didSubscription2Fire = true;
                Interlocked.Increment(ref subscriptionFireCount);
                subscription2Payload = sr;
            });
            var orig1Address3 = await ChangeAndChangeBack(dataAccess, unusedAddress1, subscription1Action);
            var orig2Address3 = await ChangeAndChangeBack(dataAccess, unusedAddress2, subscription2Action);

            //Make sure subscription is fired
            Assert.True(didSubscription1Fire);
            if (subscriptionFireCount < 2)
            {
                await Task.Delay(10); //Give subscriptions time to fire, if needed
                output.WriteLine("Waited for 2nd subscription to fire.");
            }
            Assert.NotNull(subscription1Payload);
            Assert.Equal(unusedAddress1.Id, subscription1Payload.Result.Id);
            Assert.Equal(orig1Address3, subscription1Payload.Result.Address3);

            Assert.True(didSubscription2Fire);

            Assert.NotNull(subscription1Payload);

            var maxWaits = 5;
            while (subscriptionFireCount < 4 && maxWaits > 0)
            {
                maxWaits--;
                await Task.Delay(10 * (5 - maxWaits));
            } //Give subscriptions time to fire, if needed
            output.WriteLine($"Waited for 4th subscription to fire {5 - maxWaits} times.");
            Assert.Equal(unusedAddress2.Id, subscription2Payload?.Result.Id);
            Assert.Equal(orig2Address3, subscription2Payload?.Result.Address3);
            //NOTE:  Testing the fire count not only confirms that it fired all 4 times, but that it never double fired.
            Assert.Equal(4, subscriptionFireCount);
        }

        private static async Task<List<Address>> GetUnusedAddresses(ServiceProvider services)
        {
            //Find an address
            var contextFactory =
                (IDbContextFactory<JamesDatabaseContext>)services.GetService(
                    typeof(IDbContextFactory<JamesDatabaseContext>))!;
            await using var ctx = await contextFactory.CreateDbContextAsync();

            var unusedAddressQuery = FormattableStringFactory.Create(@"with AddressReferences AS (
SELECT AddressId FROM dbo.LegalEntityAddress
UNION ALL	
SELECT AddressId FROM dbo.AgencyInventory
UNION ALL
SELECT Id FROM dbo.Address),
AddressReferenceCount AS(
SELECT AddressId,COUNT(*) Cnt FROM AddressReferences GROUP BY AddressId
)
SELECT TOP 2 a.*
FROM AddressReferenceCount  arc
INNER JOIN dbo.Address a ON a.Id = arc.AddressId
ORDER BY cnt, a.Modified, a.Created");
            var unusedAddresses = ctx.Addresses.FromSql(unusedAddressQuery).AsEnumerable().ToList();
            return unusedAddresses;
        }

        private async Task<string?> ChangeAndChangeBack(IDataAccess dataAccess, Address unusedAddress, Action<SubscriptionResult<Address>> subscriptionAction)
        {
            //Subscribe
            using var subscription = dataAccess.AddressModified(unusedAddress.Id, subscriptionAction);
            //Change it and change it back
            var origAddress3 = unusedAddress.Address3;
            var newAddress3 = string.IsNullOrWhiteSpace(origAddress3) ? "UnitTestValue" : null;
            var newAddress = ThisToThat.ToEntityType<Address>(unusedAddress);//Clones the original
            newAddress.Address3 = newAddress3;
            var identifier = Guid.NewGuid().ToString();
            var changeAddress = await dataAccess.SetAddress(newAddress, identifier);
            Assert.True(changeAddress.Success);
            output.WriteLine("Address ID {0} changed", unusedAddress.Id);
            var resetAddress = await dataAccess.SetAddress(unusedAddress, identifier);
            Assert.True(resetAddress.Success);
            output.WriteLine($"Address ID {unusedAddress.Id} changed back");
            return origAddress3;
        }

        protected ServiceProvider CreateServer(SubscriptionOptions? options = null)
        {
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables()
            //    .Build();
            var scsb = new SqlConnectionStringBuilder
            {
                DataSource = "usilg01-dwd057",
                InitialCatalog = "JamesDev",
                TrustServerCertificate = true,
                MultipleActiveResultSets = true,
                UserID = "JamesUser"
            };
            var testConnectionString = scsb.ConnectionString + ";Password = $ecure4D3v3l0pment";
            var services = new ServiceCollection();
            services
                .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
                {
                    o.EnableDetailedErrors();
                    o.UseSqlServer(testConnectionString);
                    o.EnableDetailedErrors();
                    o.EnableSensitiveDataLogging();//TODO: Disable in production environment
                    //o.UseMemoryCache()
                });
            services.AddGraphQLServer()
                    .AddQueryType<Query>()
                    .RegisterDbContextFactory<JamesDatabaseContext>()
                    .AddSubscriptionType<Subscription>()
                    .AddInMemorySubscriptions(options)
                    .AddMutationConventions();

            services.AddScoped<Query>();
            services.AddScoped<AgencyMutation>();
            services.AddSingleton(typeof(IUserShared), typeof(TestUserShared));
            services.AddSingleton(typeof(ILogger), typeof(NullLogger));
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddScoped<ILoggingShared, LoggingShared>();
            services.AddScoped<ILoggingService, ServerLoggingService>();
            services.AddScoped<IDataAccess, ServerDataAccess>();
            return services.BuildServiceProvider();
        }

        protected ServiceProvider CreateServer(Action<IRequestExecutorBuilder> configure)
        {
            var serviceCollection = new ServiceCollection();
            var graphqlBuilder = serviceCollection.AddGraphQL();

            configure(graphqlBuilder);
            //ConfigurePubSub(graphqlBuilder);

            return serviceCollection.BuildServiceProvider();
        }
    }

    public class TestContextFactory : IDbContextFactory<JamesDatabaseContext>
    {
        private static TestContextFactory? _instance;
        internal static TestContextFactory Instance => _instance ??= new TestContextFactory();

        public JamesDatabaseContext CreateDbContext()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            var options = new DbContextOptionsBuilder<JamesDatabaseContext>();
            //TODO: Change to use a Unit test specific local db
            options.UseSqlServer(config.GetConnectionString("James"));

            return new JamesDatabaseContext(options.Options);
        }
    }

    public class TestUserShared : IUserShared
    {
        public async Task<SiteUserInfo> GetCurrentUser()
        {
            return await Task.FromResult(_fakeTestUser);
        }

        public async Task<SiteUserInfo> GetUserInfoAsync(string jwtToken)
        {
            return await Task.FromResult(_fakeTestUser);
        }
        private readonly SiteUserInfo _fakeTestUser = new SiteUserInfo
        {
            EntraId = "entraId",
            FirstName = "first",
            FullName = "full",
            Email = "test@fake.com",
            JWT = "jwt"
        };
    }
}