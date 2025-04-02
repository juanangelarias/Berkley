using James.Data.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace James.Test.Shared;

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