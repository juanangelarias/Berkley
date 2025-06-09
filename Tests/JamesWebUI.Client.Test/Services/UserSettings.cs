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
using James.Shared.Server;
using James.Shared.Server.Kong0;
using James.Test.Shared;
using JamesWebUI.Client.Services;
using JamesWebUI.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace JamesWebUI.Client.Test.Services
{
    public class UserSettings
    {
        public UserSettings(ITestOutputHelper output)
        {
            Output = output;
            lock (typeof(UserSettings))
                _lazyServiceProvider ??= new Lazy<ServiceProvider>(() => CreateServer());
        }

        [Fact]
        public async Task GetUserSettings()
        {
            var uss = GetUserSettingsService();
            //var dataAccess = _lazyServiceProvider!.Value.GetService<IDataAccess>();
            //Get user settings as baseline
            var settings = await uss.GetAllUserSettingsAsync();
            Assert.NotNull(settings);
            Assert.True(settings.Any());
        }

        [Fact]
        public async Task SetUserSetting()
        {
            var uss = GetUserSettingsService();
            //Add a key/value and confirm it 
            var testKey = "UnitTest12345";
            var testValue = "UnitTest12345Value";
            await uss.SetUserSettingAsync(testKey, testValue);
            var setSetting = await uss.GetUserSettingAsync(testKey);
            Assert.NotNull(setSetting);
            Assert.Equal(testValue, setSetting);

            //Remove the key and confirm it is gone
            //HACK: This test will break if somebody adds a default value to this key
            await uss.SetDefaultUserSettingAsync(testKey, null);
            await uss.SetUserSettingAsync(testKey, null);
            setSetting = await uss.GetUserSettingAsync(testKey, true);
            Assert.Null(setSetting);
        }

        [Fact]
        public async Task SetDefaultUserSetting()
        {
            var uss = GetUserSettingsService();
            //Add a key/value and confirm it 
            var testKey = "UnitTest123456";
            var testValue = "UnitTest123456Value";
            await uss.SetDefaultUserSettingAsync(testKey, testValue);
            await uss.SetUserSettingAsync(testKey, null);
            var setSetting = await uss.GetUserSettingAsync(testKey);
            Assert.NotNull(setSetting);
            Assert.Equal(testValue, setSetting);

            //Remove the key and confirm it is gone
            await uss.SetDefaultUserSettingAsync(testKey, null);
            setSetting = await uss.GetUserSettingAsync(testKey, true);
            Assert.Null(setSetting);
        }

        private ITestOutputHelper Output { get; init; }
        private static UserSettingService GetUserSettingsService() =>
            _lazyServiceProvider!.Value.GetService<UserSettingService>()!;
        private static Lazy<ServiceProvider>? _lazyServiceProvider;

        protected ServiceProvider CreateServer(SubscriptionOptions? options = null)
        {
            var scsb = new SqlConnectionStringBuilder
            {
                DataSource = "usilg01-dwd217",
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
                    o.EnableSensitiveDataLogging(); //TODO: Disable in production environment
                });
            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .RegisterDbContextFactory<JamesDatabaseContext>()
                .AddSubscriptionType<Subscription>()
                .AddInMemorySubscriptions(options)
                .AddMutationConventions();
            var kong0TokenUrl =
                new Uri(config["Kong0:token_url"] ?? "https://dev-auth.wrberkley.auth0.com/oauth/token");
            services.AddHttpClient("P8FileNetTokens",
                    client =>
                    {
                        client.BaseAddress = kong0TokenUrl;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler()
                    { ClientCertificateOptions = ClientCertificateOption.Automatic })
                .AddTraceContentLogging();

            services.AddScoped<Query>();
            services.AddScoped<AgencyMutation>();
            services.AddScoped<AccountMutation>();
            services.AddScoped<GeneralMutation>();
            services.AddScoped<ObligeeMutation>();
            services.AddScoped<IUserShared, TestUserShared>();
            services.AddScoped<IHttpContextAccessor, TestHttpContextAccessor>();
            services.AddScoped<ImagingKong0Helper>();
            services.AddScoped<ServerImagingAccess>();
            services.AddScoped<AgencyMutation>();
            services.AddScoped<UserSettingService>();
            services.AddSingleton(typeof(ILogger), typeof(NullLogger));
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddScoped<ILoggingShared, LoggingShared>();
            services.AddScoped<ILoggingService, ServerLoggingService>();
            services.AddScoped<IDataAccess, ServerDataAccess>();
#pragma warning disable CA1416
            services.SetupImagingForKong(config);
#pragma warning restore CA1416
            services.AddScoped<ILocalStorageService, TestLocalStorageService>();
            services.AddScoped<UserSettingService>();

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

    public class TestHttpContextAccessor : IHttpContextAccessor
    {
        private HttpContext? _httpContext;
        public HttpContext? HttpContext
        {
            get
            {
                if (null == _httpContext)
                {
                    _httpContext = new DefaultHttpContext();
                    var testUserIdentity = new GenericIdentity("TestUser1234");
                    testUserIdentity.AddClaim(new Claim("nickname", "TestUser1234"));
                    _httpContext.User = new ClaimsPrincipal(testUserIdentity);
                }
                return _httpContext;
            }
            set => _httpContext = value;
        }
    }
}
