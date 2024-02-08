using James.Data.Server.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .RegisterDbContext<JamesDatabaseContext>(DbContextKind.Pooled);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Authorization ALWAYS after Authentication, both after UseRouting(); 

app.UseCors();

app.MapGraphQL("/graphql");
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
