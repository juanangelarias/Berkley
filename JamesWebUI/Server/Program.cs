using James.Data.Server.Model;
using JamesWebUI.Client;
using JamesWebUI.Client.Components;
using JamesWebUI.Client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Radzen;



var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Auth0UserInfo",
    client => client.BaseAddress = new Uri("https://apps-sbox.wrberkley.auth0.com/"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("Auth0UserInfo"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:ApiIdentifier"];
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddPooledDbContextFactory<JamesDatabaseContext>(o =>
    {
        o.EnableDetailedErrors();
        o.UseSqlServer(config.GetConnectionString("James"));
        //o.UseMemoryCache()
    });
builder.Services
    .AddGraphQLServer()
    .AddQueryType<JamesWebUI.Server.GraphQL.Queries.Query>()
    .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled);

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

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseWebAssemblyDebugging();
//}
//else
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Authorization ALWAYS after Authentication, both after UseRouting(); 
app.UseAntiforgery();

app.UseCors();


app.MapRazorPages();
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode();

app.MapGraphQL("/graphql");

app.Run();
