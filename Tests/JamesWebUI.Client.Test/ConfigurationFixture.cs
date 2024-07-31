namespace JamesWebUI.Client.Test
{
    public class ConfigurationFixture : IDisposable
    {
        //private IConfigurationRoot? _config;

        public IConfigurationRoot Config => InternalConfiguration.Config;
            //_config ?? (_config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables()
            //    .Build());

        public void Dispose()
        {
            // TODO release managed resources here
        }
    }

    internal static class InternalConfiguration
    {
        //HACK:  This is how to allow the ClassFixtures to access the config.  Wish there was a cleaner way.
        private static IConfigurationRoot? _config;

        public static IConfigurationRoot Config =>
            _config ?? (_config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build());
    }
}
