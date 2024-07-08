using James.Data.Server.GraphQL;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Data.Server.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using HotChocolate.Execution.Configuration;
using HotChocolate.Subscriptions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace James.Data.Server
{
    public class SubscriptionTests
    {
        [Fact]
        public async void OnAddressModified()
        {
            await using var services = CreateServer<Subscription>();
            var graphQlQuery = new Query();
            var agencyMutation = new AgencyMutation();

            //var dataAccess = new ServerDataAccess(TestContextFactory.Instance, graphQlQuery, agencyMutation, );
            ////Subcribe
            //using var subscription = 
            //using(var ctx = .CreateDbContext())
            //{
            //    //TODO: Get an address
            //    //TODO: Modify the address
            //}
            //TODO: Make sure subscription is fired

        }

        protected ServiceProvider CreateServer<TSubscriptionType>(SubscriptionOptions? options = null) where TSubscriptionType : class
        {
            var services = new ServiceCollection();
            services.TryAddSingleton(options ?? new SubscriptionOptions());
            //services.TryAddSingleton<InMemoryPubSub>();
            //services.TryAddSingleton<ITopicEventSender>(
            //    sp => sp.GetRequiredService<InMemoryPubSub>());
            //services.TryAddSingleton<ITopicEventReceiver>(
            //    sp => sp.GetRequiredService<InMemoryPubSub>());
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

    public class TestContextFactory:IDbContextFactory<JamesDatabaseContext>
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