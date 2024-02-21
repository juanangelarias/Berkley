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
    .AddSubscriptionType<Subscription>()
    .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled)
    .AddMutationConventions()
    .AddInMemorySubscriptions()
    ;
builder.Services
    .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
{
    o.EnableDetailedErrors();
    o.UseSqlServer(config.GetConnectionString("James"));
});
builder.Services.AddCors(options =>
{
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
//For subscriptions
//app.UseRouting();
//app.UseWebSockets();
//app.UseEndpoints(endpoints =>
//{
//    _ = endpoints.MapGraphQL();
//});

app.Run();
