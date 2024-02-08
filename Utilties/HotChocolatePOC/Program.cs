using HotChocolatePOC;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<BankMutationType>()
    .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled)
    .AddMutationConventions()
    .AddInMemorySubscriptions();
builder.Services
    .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
{
    o.EnableDetailedErrors();
    o.UseSqlServer(config.GetConnectionString("James"));
    //o.UseMemoryCache()
});
builder.Services.AddCors(options =>
{
    //options.AddPolicy(name: MyAllowSpecificOrigins,
    //    policy =>
    //    {
    //        policy.AllowAnyOrigin();
    //        policy.WithOrigins("https://localhost:7265", "http://localhost:5130");
    //    });
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        //policy.WithOrigins("https://localhost:7265/");
    });
});

var app = builder.Build();

app.UseCors();
app.MapGraphQL("/graphql");
app.MapGet("/HelloWorld", () => "Hello World!");
//app.MapGet("/", () => "Go to /graphql");
app.MapGet("/", req =>
{
    req.Response.Redirect("/graphql");
    return Task.FromResult("Go to /graphql");
});

app.Run();
