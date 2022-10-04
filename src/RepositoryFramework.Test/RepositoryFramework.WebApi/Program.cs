using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using RepositoryFramework;
using RepositoryFramework.WebApi;
using RepositoryFramework.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
#pragma warning disable S125
//builder.Services.AddRepositoryInMemoryStorage<User>()
//.PopulateWithRandomData(x => x.Email!, 120, 5)
// Sections of code should not be commented out
//.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
//builder.Services.AddRepository<IperUser, string, IperRepositoryStorage>()
var configurationSection = builder.Configuration.GetSection("AzureAd");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(configurationSection);

builder.Services.AddRepositoryInMemoryStorage<IperUser, string>()
    .AddBusinessBeforeInsert<IperRepositoryBeforeInsertBusiness>()
    .WithInMemoryCache(x =>
    {
        x.ExpiringTime = TimeSpan.FromMilliseconds(100_000);
    });
builder.Services.AddRepositoryInMemoryStorage<SuperUser, string>()
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
builder.Services.AddRepositoryInMemoryStorage<SuperiorUser, string>()
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com")
.WithPattern(x => x.Port, @"[1-9]{3,4}");
builder.Services.AddRepositoryInMemoryStorage<Animal, AnimalKey>();
builder.Services.AddRepositoryInMemoryStorage<Car, Guid>();
builder.Services.AddRepositoryInMemoryStorage<Car2, Range>();
builder.Services.AddApiFromRepositoryFramework()
    .WithName("Repository Api")
    .WithSwagger()
    .WithDocumentation()
    .ConfigureAzureActiveDirectory(builder.Configuration);

builder.Services
    .AddAuthorization()
    .AddServerSideBlazor(opts => opts.DetailedErrors = true)
    .AddMicrosoftIdentityConsentHandler();
//builder.Services
//    .AddRepositoryInTableStorage<User, string>(builder.Configuration["ConnectionString:Storage"]);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["ConnectionString:Redis"];
    options.InstanceName = "SampleInstance";
});
//builder.Services
//    .AddRepositoryInBlobStorage<User, string>(builder.Configuration["ConnectionString:Storage"])
//    .WithInMemoryCache(x =>
//    {
//        x.RefreshTime = TimeSpan.FromSeconds(60);
//        x.Methods = RepositoryMethod.Get | RepositoryMethod.Insert | RepositoryMethod.Update | RepositoryMethod.Delete;
//    })
//    .WithDistributedCache(x =>
//    {
//        x.RefreshTime = TimeSpan.FromSeconds(120);
//        x.Methods = RepositoryMethod.All;
//    });
//.WithBlobStorageCache(builder.Configuration["ConnectionString:Storage"], settings: x =>
//{
//    x.RefreshTime = TimeSpan.FromSeconds(120);
//    x.Methods = RepositoryMethod.All;
//});
builder.Services
    .AddRepositoryInCosmosSql<User, string>(
    builder.Configuration["ConnectionString:CosmosSql"],
    "BigDatabase");

#pragma warning restore S125 // Sections of code should not be commented out

var app = builder.Build();
app.Services.Populate();

app.UseHttpsRedirection();
app.UseApiFromRepositoryFramework()
    .WithDefaultAuthorization();

app.UseAuthentication();
app.UseAuthorization();
#pragma warning disable S125 // Sections of code should not be commented out
//.SetPolicy(RepositoryMethod.Query)
//.Empty()
//.SetPolicy(RepositoryMethod.Delete)
//.With("Admin")
//.With("Other")
//.And()
//.Finalize();
#pragma warning restore S125 // Sections of code should not be commented out


app.Run();
