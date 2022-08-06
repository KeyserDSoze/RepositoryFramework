using RepositoryFramework;
using RepositoryFramework.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
#pragma warning disable S125
//builder.Services.AddRepositoryInMemoryStorage<User>()
//.PopulateWithRandomData(x => x.Email!, 120, 5)
// Sections of code should not be commented out
//.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
builder.Services.AddRepositoryInMemoryStorage<SuperUser, string>()
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
builder.Services.AddRepositoryInMemoryStorage<IperUser, string>()
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com")
.WithPattern(x => x.Port, @"[1-9]{3,4}");
builder.Services.AddRepositoryInMemoryStorage<Animal, AnimalKey>();
builder.Services.AddRepositoryInMemoryStorage<Car, Guid>();
builder.Services.AddRepositoryInMemoryStorage<Car2, Range>();
//builder.Services
//    .AddRepositoryInTableStorage<User, string>(builder.Configuration["Storage:ConnectionString"]);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "SampleInstance";
});
//builder.Services
//    .AddRepositoryInBlobStorage<User, string>(builder.Configuration["Storage:ConnectionString"])
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
//.WithBlobStorageCache(builder.Configuration["Storage:ConnectionString"], settings: x =>
//{
//    x.RefreshTime = TimeSpan.FromSeconds(120);
//    x.Methods = RepositoryMethod.All;
//});
builder.Services
    .AddRepositoryInCosmosSql<User, string>(
    x => x.Email!,
    builder.Configuration["CosmosSql:ConnectionString"],
    "BigDatabase");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#pragma warning restore S125 // Sections of code should not be commented out

var app = builder.Build();
app.Services.Populate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddApiForRepositoryFramework()
    .WithNoAuthorization();

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