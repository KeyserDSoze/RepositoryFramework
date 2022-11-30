using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using RepositoryFramework.Test.Domain;
using RepositoryFramework.Test.Infrastructure.EntityFramework;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models.Internal;
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
builder.Services.AddRepositoryInMemoryStorage<SuperiorUser, string>(null, x =>
{
    x.IsNotExposable = true;
})
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com")
.WithPattern(x => x.Port, @"[1-9]{3,4}");
builder.Services.AddRepositoryInMemoryStorage<Animal, AnimalKey>();
builder.Services.AddRepositoryInMemoryStorage<Car, Guid>();
builder.Services.AddRepositoryInMemoryStorage<Car2, Range>();
builder.Services
    .AddUserRepositoryWithDatabaseSqlAndEntityFramework(builder.Configuration);
builder.Services.AddApiFromRepositoryFramework()
    .WithDescriptiveName("Repository Api")
    .WithSwagger()
    .WithDocumentation()
    .WithDefaultCorsWithAllOrigins();
//.ConfigureAzureActiveDirectory(builder.Configuration);

builder.Services
    .AddAuthorization()
    .AddServerSideBlazor(opts => opts.DetailedErrors = true)
    .AddMicrosoftIdentityConsentHandler();
//builder.Services
//    .AddRepositoryInTableStorage<User, string>(builder.Configuration["ConnectionString:Storage"]);
builder.Services.AddRepositoryInBlobStorage<BigAnimal, int>(x =>
{
    x.ConnectionString = builder.Configuration["ConnectionString:Storage"];
});

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
    .AddRepositoryInCosmosSql<CreativeUser, string>(
    x =>
    {
        x.ConnectionString = builder.Configuration["ConnectionString:CosmosSql"];
        x.DatabaseName = "BigDatabase";
    })
    .WithId(x => x.Email!);

#pragma warning restore S125 // Sections of code should not be commented out

var app = builder.Build();
await app.Services.WarmUpAsync();

app.UseHttpsRedirection();
app.UseApiFromRepositoryFramework()
    .WithNoAuthorization();
//.WithDefaultAuthorization();

//app.UseAuthentication();
//app.UseAuthorization();
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
