using ApplicationLog;
using FileNetP8SoapService;
using James.Data.Imaging;
using James.Data.Server.Model;
using James.Shared;
using James.Shared.Data;
using James.Shared.Imaging;
using James.Shared.Model;
using James.Shared.Server;
using James.Shared.Server.Kong0;
using JamesWebUI.Server.SharedServices;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using ImagingExtensions = James.Data.Imaging.ImagingExtensions;

namespace James.Data.Server.Test
{
    [SupportedOSPlatform("windows")]
    public class ImagingRepositoryTests(ITestOutputHelper output)
    {
        [Fact]
        [SupportedOSPlatform("windows")]
        public async Task GetPropertyMappings()
        {
            if (!Environment.MachineName.StartsWith("USUBL"))
                //Build Server does not have network access to the imaging servers.
                return;
            var services = CreateServer();
            var serverImagingAccess = (ServerImagingAccess)services.GetService(typeof(ServerImagingAccess))!;
            var categoriesToTest = 6;
            var propertiesAvailableByCategory = new Dictionary<ImagingDocumentCategory, ImagingProperty[]>();
            foreach (var imagingDocumentCategory in Enumerable.Range(1, categoriesToTest)
                         .Cast<ImagingDocumentCategory>())
                propertiesAvailableByCategory[imagingDocumentCategory] =
                    await serverImagingAccess.GetAvailableImagingPropertiesAsync(imagingDocumentCategory);
            Assert.Equal(categoriesToTest, propertiesAvailableByCategory.Count);
            Assert.Contains(propertiesAvailableByCategory, p => p.Value.Length > 0);
            Assert.Equal(propertiesAvailableByCategory.Count, serverImagingAccess.ValidProperties.Count);
            for (var i = 1; i <= propertiesAvailableByCategory.Count; i++)
                Assert.Equal(propertiesAvailableByCategory[(ImagingDocumentCategory)i].Length,
                    serverImagingAccess.ValidProperties[(ImagingDocumentCategory)i].Length);
            output.WriteLine(string.Join("\r\n",
                propertiesAvailableByCategory.Select(
                    pabc =>
                        string.Format("Doc Class = {0}\r\nProperties Available:\r\n\t{1}", pabc.Key.DocumentCategory(),
                            string.Join("\r\n\t",
                                pabc.Value.Select(
                                    v =>
                                        string.Format("{0} ({1}){2}", v.Name, v.DataType,
                                            v.ReadOnly ? " - READONLY" : string.Empty)))))));
        }
        public Tuple<ImagingDocumentCategory, Guid>[] TestDocList =>
        [
            new Tuple<ImagingDocumentCategory, Guid>(ImagingDocumentCategory.Account,
                Guid.Parse("00CB1A02-2833-5A0E-8B7F-4F90FF540B04")),
            new Tuple<ImagingDocumentCategory, Guid>(ImagingDocumentCategory.Agency,
                Guid.Parse("00CEA641-2833-5A0E-8B7F-4F90FF540B04")),
            new Tuple<ImagingDocumentCategory, Guid>(ImagingDocumentCategory.CommBid,
                Guid.Parse("00CAD20C-2833-5A0E-8B7F-4F90FF540B04")),
            new Tuple<ImagingDocumentCategory, Guid>(ImagingDocumentCategory.Bond,
                Guid.Parse("00CDB116-2833-5A0E-8B7F-4F90FF540B04"))
        ];

        /// <summary>
        /// Tests the p8 int docs by string doc classifications.
        /// </summary>
        [Fact]
        public async Task GetP8IntDocs1()
        {
            if (!Environment.MachineName.StartsWith("USUBL"))
                //Build Server does not have network access to the imaging servers.
                return;
            var services = CreateServer();
            var serverImagingAccess = (ServerImagingAccess)services.GetService(typeof(ServerImagingAccess))!;
            var docs = TestDocList.Select(d => new Tuple<ImagingDocumentCategory, Guid>(d.Item1, d.Item2)).ToArray();
            List<(Stream, string, string)> docsFound = new();
            try
            {
                docsFound.AddRange(docs.Select(d => serverImagingAccess.GetFileStreamAsync(d.Item2, d.Item1).Result).ToList());
                Assert.NotNull(docsFound);
                Assert.True(docsFound.Any());
                Assert.True(docsFound.All(df => df.Item1.Length > 9));
                Assert.NotNull(docs);
                var fndDocs = docsFound.ToArray();
                Assert.NotNull(fndDocs);
                Assert.Equal(docs.Length, fndDocs.Length);
            }
            finally
            {
                foreach (var fileStream in docsFound)
                    await fileStream.Item1.DisposeAsync();
            }
        }


        /// <summary>
        /// Tests the p8 int docs by ImgDocCategory enum.
        /// </summary>
        [Fact]
        public void GetP8IntDocs2()
        {
            if (!Environment.MachineName.StartsWith("USUBL"))
                //Build Server does not have network access to the imaging servers.
                return;
            var services = CreateServer();
            var serverImagingAccess = (ServerImagingAccess)services.GetService(typeof(ServerImagingAccess))!;
            //BSGAccounting: {72E95C6E-0F26-4A9B-A1ED-11B11FCD6FC9}
            //BSGAgency: {82437C64-CFC6-4335-9AC9-9FD062F256B9}
            //BSGBilling: {B0F428CA-8B01-462D-BE42-0391F59B328F}
            //BSGCWS: {67D80D23-6CB5-4DEA-B8B7-8F13770C3E39}
            //BSGUnderwriting: {0C01DE29-A6F0-4191-9452-F445244EBB52}
            var docs = TestDocList;
            var fields = ImagingAccessBase.DocumentPropertyFields.ToList();
            fields.Remove(ImagingAccessBase.ScanDate);
            var fieldList = string.Join(",", fields);
            var docsFound =
                docs.Select(
                    d =>
                        new ImagingSearchCriteria()
                        {
                            DocClass = d.Item1.DocumentCategory(),
                            MaxResults = 10,
                            Fields = fieldList,
                            WhereClause = $"Id = '{d.Item2}'"
                        }).SelectMany(sc =>
                    serverImagingAccess.SearchDocumentsAsync(sc).Result).ToArray();
            Assert.NotNull(docsFound);
            foreach (var result in docsFound)
                output.WriteLine(result.ToString());
            Assert.Equal(docs.Length, docsFound.Length);
        }

        protected ServiceProvider CreateServer()
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
            Kong0HelperBase.DetailedLogging =
                bool.TryParse(config["Kong0:http_client_trace_logging"], out var traceLogging) &&
                 traceLogging;
            services
                .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
                {
                    o.EnableDetailedErrors();
                    o.UseSqlServer(testConnectionString);
                    o.EnableDetailedErrors();
                    o.EnableSensitiveDataLogging();//TODO: Disable in production environment
                });
            var kong0TokenUrl = new Uri(config["Kong0:token_url"] ?? "https://dev-auth.wrberkley.auth0.com/oauth/token");
            services.AddHttpClient("P8FileNetTokens",
                client =>
                {
                    client.BaseAddress = kong0TokenUrl;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler() { ClientCertificateOptions = ClientCertificateOption.Automatic })
                .AddTraceContentLogging();
            services.AddSingleton(typeof(IUserShared), typeof(TestUserShared));
            services.AddSingleton(typeof(ILogger), typeof(NullLogger));
            services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            services.AddScoped<ILoggingShared, LoggingShared>();
            services.AddScoped<ILoggingService, ServerLoggingService>();
            services.AddScoped<IDataAccess, ServerDataAccess>();
            services.SetupImagingForKong(config);


            services.AddLogging(c=>c
                //builder.Logging
                .AddConsole()
#if DEBUG
                .AddDebug()
#endif
                .AddSerilog(new LoggerConfiguration()
                    .Enrich.WithApplicationInfo(config["ApplicationId"]!, "James")
            .ReadFrom.Configuration(config)
                    .WriteTo.TestOutput(output)
                    .CreateLogger()));
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;

            });
            return services.BuildServiceProvider();
        }


        /// <summary>
        /// Tests retrieving the p8 docs
        /// </summary>
        [Fact]
        public void RetrieveP8IntDocs2()
        {
            if (!Environment.MachineName.StartsWith("BSGIALT"))
                //Build Server does not have network access to the imaging servers.
                return;
            var services = CreateServer();
            var serverImagingAccess = (ServerImagingAccess)services.GetService(typeof(IImagingAccess))!;
            var docs = TestDocList;
            var fields = ImagingAccessBase.DocumentPropertyFields.ToList();
            fields.Remove(ImagingAccessBase.ScanDate);
            var fieldList = string.Join(",", fields);
            var docsFound =
                docs.Select(
                    d =>
                        new searchCriteria
                        {
                            docClass = d.Item1.DocumentCategory(),
                            maxResults = 2.ToString(),
                            fields = fieldList,
                            whereClause = $"Id = '{d.Item2}'"
                        }).SelectMany(sc =>
                    serverImagingAccess.SearchDocumentsAsync(sc.ToImagingSearchCriteria()).Result).ToArray();
            Assert.NotNull(docsFound);
            foreach (var result in docsFound)
                Debug.WriteLine(ImagingExtensions.DocumentDebugText(result));
            Assert.Equal(docs.Length, docsFound.Length);
        }

        /// <summary>
        /// Searches the company documents.
        /// </summary>
        [Fact]
        public async Task SearchCompanyDocuments()
        {
            if (!Environment.MachineName.StartsWith("BSGIALT"))
                //Build Server does not have network access to the imaging servers.
                return;
            var services = CreateServer();
            var serverImagingAccess = (ServerImagingAccess)services.GetService(typeof(IImagingAccess))!;
            var resultList = new List<ImagingDocument>();
            for (var docClass = 1; docClass < 6; docClass++)
            {
                var validProperties = serverImagingAccess.ValidProperties[(ImagingDocumentCategory)docClass];
                var field1 = validProperties.Any(p => p.Name == "AccountID") ? "AccountID" : validProperties[0].Name;
                var field2 = validProperties.Any(p => p.Name == "CompanyNo") ? "CompanyNo" : validProperties.First(vp => vp.Name != "AccountID").Name;
                var criteria = new ImagingSearchCriteria()
                {
                    DocClass = ((ImagingDocumentCategory)docClass).DocumentCategory(),
                    MaxResults = 10,
                    Fields = field1 + "," + field2,
                    WhereClause = "CompanyNo='27'"
                };
                resultList.AddRange(await serverImagingAccess.SearchDocumentsAsync(criteria));
            }

            Assert.NotNull(resultList);
            Assert.True(resultList.Any());
            foreach (var result in resultList)
                Debug.WriteLine(ImagingExtensions.DocumentDebugText(result));
        }

        /// <summary>
        /// Searches the documents test.
        /// </summary>
        [Fact]
        public async Task SearchDocumentsTest()
        {
            if (!Environment.MachineName.StartsWith("BSGIALT"))
                //Build Server does not have network access to the imaging servers.
                return;
            var services = CreateServer();
            var serverDataAccess = (ServerDataAccess)services.GetService(typeof(IDataAccess))!;
            var searchResult = await serverDataAccess.SearchDocuments("322", ImagingDocumentCategory.Agency);
            Assert.NotNull(searchResult.Data);
            Assert.True(searchResult.Data.Any());
        }

    }
}
