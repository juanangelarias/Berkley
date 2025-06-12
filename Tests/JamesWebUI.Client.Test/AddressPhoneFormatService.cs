using ApplicationLog;
using Blazored.LocalStorage;
using HotChocolate.Subscriptions;
using James.Data.Imaging;
using James.Data.Server;
using James.Data.Server.GraphQL;
using James.Data.Server.GraphQL.Mutations;
using James.Data.Server.GraphQL.Queries;
using James.Data.Server.Model;
using James.Shared;
using James.Shared.Data;
using James.Shared.Model;
using James.Shared.Server;
using James.Shared.Server.Kong0;
using James.Test.Shared;
using JamesWebUI.Client.Services;
using JamesWebUI.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace JamesWebUI.Client.Test
{
    public class AddressPhoneFormatServiceTests
    {
        public AddressPhoneFormatServiceTests(ITestOutputHelper output)
        {
            Output = output;
            lock (typeof(AddressPhoneFormatServiceTests))
                _lazyServiceProvider ??= new Lazy<ServiceProvider>(()=>CreateServer());
        }
        [Theory]
        [MemberData(nameof(AddressesWithNoCountry))]
        public async Task DefaultPostalCodesToUs(Address address, string correctResult)
        {
            var apfs = GetAddressPhoneNumberService();
            var computedResult = await apfs.FormatPostalCodeForCountryAsync(address);
            Assert.Equal(correctResult, computedResult);
        }

        [Theory]
        [MemberData(nameof(AddressesWithCountry))]
        public async Task PostalCodesFormattedByCountry(Address address, string correctResult)
        {
            var apfs = GetAddressPhoneNumberService();
            var computedResult = await apfs.FormatPostalCodeForCountryAsync(address);
            Assert.Equal(correctResult, computedResult);
        }

        [Theory]
        [MemberData(nameof(PhoneNumbersWithNoCountry))]
        public async Task DefaultPhoneNumberToUs(PhoneNumber phoneNumber, string correctResult)
        {
            var apfs = GetAddressPhoneNumberService();
            var computedResult = await apfs.FormatPhoneNumberForCountryAsync(phoneNumber);
            Assert.Equal(correctResult, computedResult);
        }

        [Theory]
        [MemberData(nameof(PhoneNumbersWithCountry))]
        public async Task PhoneNumbersFormattedByCountry(PhoneNumber phoneNumber, string correctResult)
        {
            var apfs = GetAddressPhoneNumberService();
            var computedResult = await apfs.FormatPhoneNumberForCountryAsync(phoneNumber);
            Assert.Equal(correctResult, computedResult);
        }

        [Theory]
        [MemberData(nameof(Cities))]
        public async Task AddressFinalLine(Address address, string correctResult)
        {
            var apfs = GetAddressPhoneNumberService();
            var computedResult = await apfs.GetAddressFinalLineAsync(address);
            Assert.Equal(correctResult, computedResult);
        }

        //public async Task Get 

        public static IEnumerable<object[]> AddressesWithNoCountry =>
            new List<object[]>
            {
                new object[] { new Address { PostalCode = "12345" }, "12345" },
                new object[] { new Address { PostalCode = "123456789" }, "12345-6789" },
                //Null zipcode should translate to empty string.
                new object[] { new Address(),""},
                //Garbage in, garbage out should still work with no country
                new object[]{new Address{PostalCode = "123456"}, "123456"}
            };
        public static IEnumerable<object[]> AddressesWithCountry =>
            new List<object[]>
            {
                new object[] { new Address { PostalCode = "12345", StateCodeNavigation = new State{CountryCode = "DE"}}, "12345" },
                new object[] { new Address { PostalCode = "AB12DE", StateCodeNavigation = new State{CountryCode = "GB"}}, "AB1 2DE" },
                new object[] { new Address { PostalCode = "QA123DE", StateCodeNavigation = new State{CountryCode = "GB"}}, "QA12 3DE" },
                new object[] { new Address { PostalCode = "A1B2DE", StateCodeNavigation = new State{CountryCode = "GB"}}, "A1B 2DE" },
                new object[] { new Address { PostalCode = "A123DE", StateCodeNavigation = new State{CountryCode = "GB"}}, "A12 3DE" },
                new object[] { new Address { PostalCode = "AB12345", StateCodeNavigation = new State{CountryCode = "IE"} }, "AB12345" },
                new object[] { new Address { PostalCode = "A912345", StateCodeNavigation = new State{CountryCode = "IE"} }, "A91 2345" },
                new object[] { new Address { PostalCode = "78912345", StateCodeNavigation = new State{CountryCode = "BR"} }, "78912-345" },
                //Null zipcode should translate to empty string.
                new object[]{new Address(),""},
                //Garbage in, garbage out should still work with country
                new object[]{new Address{PostalCode = "123456", StateCodeNavigation = new State { CountryCode = "ES" } }, "123456"}
            };
        public static IEnumerable<object[]> PhoneNumbersWithNoCountry =>
            new List<object[]>
            {
                new object[] { new PhoneNumber { MainNumber = "1234567890" }, "(123)456 7890" },
                new object[] { new PhoneNumber { MainNumber = "1234567890", Extension = "5"}, "(123)456 7890 ext. 5" },
                //Garbage in, garbage out should still work with no country
                new object[]{new PhoneNumber { MainNumber = "123456"}, "123456"}
            };

        public static IEnumerable<object[]> PhoneNumbersWithCountry =>
            new List<object[]>
            {
                new object[]{new PhoneNumber{CountryCode = "971", MainNumber = "123456789"}, "+971 (1)2345 6789"},
                new object[]{new PhoneNumber{CountryCode = "971", MainNumber = "123456789", Extension = "23"}, "+971 (1)2345 6789 ext. 23"},
                new object[]{new PhoneNumber{CountryCode = "55", MainNumber = "1234567890"}, "+55 (12)3456 7890"},
                new object[]{new PhoneNumber{CountryCode = "55", MainNumber = "1234567890", Extension = "23"}, "+55 (12)3456 7890 ext. 23"},
                new object[]{new PhoneNumber{CountryCode = "65", MainNumber = "12345678"}, "+65 1234 5678"},
                new object[]{new PhoneNumber{CountryCode = "65", MainNumber = "12345678", Extension = "23"}, "+65 1234 5678 ext. 23"},
                new object[]{new PhoneNumber{CountryCode = "1", MainNumber = "1234567890"}, "(123)456 7890"},
                new object[]{new PhoneNumber{CountryCode = "1", MainNumber = "1234567890", Extension = "23"}, "(123)456 7890 ext. 23"},
                //Garbage in, garbage out should still work with country
                new object[]{new PhoneNumber{CountryCode = "1", MainNumber = "123456789012", Extension = "23"}, "123456789012 ext. 23" },
                //Garbage country should format for US
                new object[]{new PhoneNumber{CountryCode = "12345", MainNumber = "1234567890"}, "(123)456 7890"},
    new object[]{new PhoneNumber{CountryCode = "1", MainNumber = "1234567890", Extension = "23"}, "(123)456 7890 ext. 23"}
            };

        public static IEnumerable<object[]> Cities =>

        new List<object[]>
        {
            new object[]{new Address{City="Des Moines", StateCode = "IA", StateCodeNavigation = new State{Name = "Iowa", Code = "IA", CountryCode = "US"}, PostalCode = "234568765"}, "Des Moines, IA 23456-8765"},
            new object[]{new Address{City="Omaha", StateCode = "NE", StateCodeNavigation = new State{Name = "Nebraska", Code = "NE", CountryCode = "US"}, PostalCode = "68106"}, "Omaha, NE 68106"},
            new object[]{new Address{City= "Santiago Papasquiaro", StateCode = "DG", StateCodeNavigation = new State{Name = "Durango", Code = "DG", CountryCode = "MX"}, PostalCode = "34635" }, "34635 Santiago Papasquiaro, DG"},
            new object[]{new Address{City= "London", StateCode = "UK", StateCodeNavigation = new State{Name = "United Kingdom", Code = "UK", CountryCode = "GB"}, PostalCode = "NW16XE" }, "London NW1 6XE" },
            new object[]{new Address{City= "Havana", StateCode = "LH", StateCodeNavigation = new State{Name = "La Habana", Code = "LH", CountryCode = "CU"}, PostalCode = "10400" }, "Havana, 10400" },
            new object[]{new Address{City= "London", StateCode = "UK", StateCodeNavigation = new State{Name = "United Kingdom", Code = "UK", CountryCode = "GB"}, PostalCode = "NW1 6XE" }, "London NW1 6XE" }
        };

        private static AddressPhoneFormatService GetAddressPhoneNumberService() =>
            _lazyServiceProvider.Value.GetService<AddressPhoneFormatService>()!;
        private static Lazy<ServiceProvider> _lazyServiceProvider = null!;

        private ITestOutputHelper Output { get; init; }
        protected ServiceProvider CreateServer(SubscriptionOptions? options = null)
        {
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
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            services
                .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
                {
                    o.EnableDetailedErrors();
                    o.UseSqlServer(testConnectionString);
                    o.EnableDetailedErrors();
                    o.EnableSensitiveDataLogging();//TODO: Disable in production environment
                });
            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .RegisterDbContextFactory<JamesDatabaseContext>()
                .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions(options)
                .AddMutationConventions();
            var kong0TokenUrl = new Uri(config["Kong0:token_url"] ?? "https://dev-auth.wrberkley.auth0.com/oauth/token");
            services.AddHttpClient("P8FileNetTokens",
                    client =>
                    {
                        client.BaseAddress = kong0TokenUrl;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler() { ClientCertificateOptions = ClientCertificateOption.Automatic })
                .AddTraceContentLogging();

            services.AddScoped<Query>();
            services.AddScoped<AgencyMutation>();
            services.AddScoped<AccountMutation>();
            services.AddScoped<GeneralMutation>();
            services.AddScoped<ObligeeMutation>();
            services.AddScoped<IUserShared, TestUserShared>();
            services.AddScoped<ImagingKong0Helper>();
            services.AddScoped<ServerImagingAccess>();
            services.AddScoped<AgencyMutation>();
            services.AddSingleton(typeof(ILogger), typeof(NullLogger));
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddScoped<ILoggingShared, LoggingShared>();
            services.AddScoped<ILoggingService, ServerLoggingService>();
            services.AddScoped<IDataAccess, ServerDataAccess>();
            services.SetupImagingForKong(config);
            services.AddScoped<ILocalStorageService, TestLocalStorageService>();
            services.AddScoped<AddressPhoneFormatService>();

            services.AddLogging(c => c
                //builder.Logging
                .AddConsole()
#if DEBUG
                .AddDebug()
#endif
                .AddSerilog(new LoggerConfiguration()
                    .Enrich.WithApplicationInfo(config["ApplicationId"]!, "James")
            .ReadFrom.Configuration(config)
                    .WriteTo.TestOutput(Output)
                    .CreateLogger()));
            return services.BuildServiceProvider();
        }
    }
}
