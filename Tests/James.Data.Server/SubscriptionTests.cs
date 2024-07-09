using James.Data.Server.GraphQL;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Data.Server.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
using HotChocolate.Data;
using HotChocolate.Execution.Configuration;
using HotChocolate.Subscriptions;
using HotChocolate.Subscriptions.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using James.Shared.Data;
using James.Shared.Server;
using James.Shared;
using James.Shared.Model;
using Microsoft.AspNetCore.Components.Forms.Mapping;

namespace James.Data.Server
{
    public class SubscriptionTests
    {
        [Fact]
        public async void OnAddressModified()
        {
            await using var services = CreateServer<Subscription>();
            var dataAccess = (IDataAccess)services.GetService(typeof(IDataAccess))!;
            var didSubscriptionFire = false;
            var subscriptionFireCount = 0;
            SubscriptionResult<Address>? subscriptionPayload = null;
            //Subcribe
            using var subscription = dataAccess.AddressModified(sr =>
            {
                didSubscriptionFire = true;
                Interlocked.Increment(ref subscriptionFireCount);
                subscriptionPayload = sr;
            });
            //Find an address
            var contextFactory =
                (IDbContextFactory<JamesDatabaseContext>) services.GetService(
                    typeof(IDbContextFactory<JamesDatabaseContext>))!;
            await using var ctx = await contextFactory.CreateDbContextAsync();

            var unusedAddressQuery = FormattableStringFactory.Create( @"with AddressReferences AS (
SELECT AddressId FROM dbo.LegalEntityAddress
UNION ALL	
SELECT AddressId FROM dbo.AgencyInventory
UNION ALL
SELECT Id FROM dbo.Address),
AddressReferenceCount AS(
SELECT AddressId,COUNT(*) Cnt FROM AddressReferences GROUP BY AddressId
)
SELECT TOP 1 a.*
FROM AddressReferenceCount  arc
INNER JOIN dbo.Address a ON a.Id = arc.AddressId
ORDER BY cnt, a.Modified, a.Created");
            var unusedAddress = ctx.Addresses.FromSql(unusedAddressQuery).AsEnumerable().First();
            //Change it and change it back
            var origAddress3 = unusedAddress.Address3;
            var newAddress3 = string.IsNullOrWhiteSpace(origAddress3) ? "UnitTestValue" : null;
            var newAddress = ThisToThat.ToEntityType<Address>(unusedAddress);//Clones the original
            newAddress.Address3 = newAddress3;
            var changeAddress = await dataAccess.SetAddress(newAddress);
            Assert.True(changeAddress.Success);

            var resetAddress = await dataAccess.SetAddress(unusedAddress);
            Assert.True(resetAddress.Success);

            //Make sure subscription is fired
            Assert.True(didSubscriptionFire);
            if (subscriptionFireCount<2)
                await Task.Delay(1); //Give 2nd subscription time to fire, if needed
            Assert.Equal(2, subscriptionFireCount);

            Assert.NotNull(subscriptionPayload);

            Assert.Equal(unusedAddress.Id, subscriptionPayload.Result.Id);
            Assert.Equal(origAddress3, subscriptionPayload.Result.Address3);
        }

        protected ServiceProvider CreateServer<TSubscriptionType>(SubscriptionOptions? options = null) where TSubscriptionType : class
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
                    .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled)
                    .AddSubscriptionType<Subscription>()
                    .AddInMemorySubscriptions(options)
                    .AddMutationConventions();
            //services.AddScoped<IUserShared, UserShared>();
            //services.AddScoped<ILoggingShared, LoggingShared>();
            services.AddScoped<Query>();
            services.AddScoped<AgencyMutation>();
            services.AddScoped<ILoggingService, ServerLoggingService>();
            services.AddScoped<IDataAccess, ServerDataAccess>();
            return services.BuildServiceProvider();
        }
        //=> CreateServer(builder =>
        //{
        //    builder
        //        .AddSubscriptionType<TSubscriptionType>()
        //        .ModifyOptions(o => o.StrictValidation = false);
        //});


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
}