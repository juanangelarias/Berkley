using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared
{
    public static class ConfigurationHelper
    {
        private static IConfigurationRoot _root;

        private static IConfigurationRoot GetConfigRoot()
        {
            if (_root == null)
            {
                var assemblyLocations = new []
                {
                    //Multiple possible locations are stated to allow the config file to be found from multiple possible execution locations.
                    Assembly.GetExecutingAssembly().Location, Assembly.GetEntryAssembly()?.Location,  
                    //Next entry require to use dotnet ef command line tools such as dotnet ef migrations NewMigrationName
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)??"", "..\\..\\..\\..\\..\\JamesWebUI\\Server", "fakeFileName")
                };
                var directoryPaths = assemblyLocations.Select(al=> Path.GetDirectoryName(al)).Where(loc => loc != null).ToArray();
    
                //Debug.Assert(directoryPath != null);
                var confileFilePaths = directoryPaths.Select(dp => Path.Combine(dp, "appsettings.json")).ToArray();
                var configFilePath = confileFilePaths.FirstOrDefault(cfp=>File.Exists(cfp));

                if (configFilePath == null)
                {
                    throw new InvalidOperationException($"Config file not found.  Directories searched: {string.Join(", ", directoryPaths)}");
                }

                IConfigurationBuilder builder = new ConfigurationBuilder();
                builder.AddJsonFile(configFilePath);

                _root = builder.Build();
            }
            return _root;
        }

        public static string ConfigGetConnectionStringByName(string connnectionStringName)
        {
            if (string.IsNullOrWhiteSpace(connnectionStringName))
            {
                throw new ArgumentException(nameof(connnectionStringName));
            }

            var root = GetConfigRoot();
            var ret = root.GetConnectionString(connnectionStringName);

            if (string.IsNullOrWhiteSpace(ret))
            {
                throw new InvalidOperationException("Config value cannot be empty");
            }

            return ret;
        }

        public static string ConfigEntryGet(string configEntryName)
        {
            if (string.IsNullOrWhiteSpace(configEntryName))
            {
                throw new ArgumentException(nameof(configEntryName));
            }

            var root = GetConfigRoot();
            var ret = root[configEntryName];

            if (string.IsNullOrWhiteSpace(ret))
            {
                throw new InvalidOperationException("Config value cannot be empty");
            }

            return ret;
        }
    }
}
