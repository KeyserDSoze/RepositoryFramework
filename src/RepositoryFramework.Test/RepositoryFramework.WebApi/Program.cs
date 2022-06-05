using RepositoryFramework.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRepositoryInMemoryStorage<User>()
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
builder.Services.AddRepositoryInMemoryStorage<SuperUser, string>()
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
builder.Services.AddRepositoryInMemoryStorage<IperUser, string, bool>(
    (x, y) => x)
.PopulateWithRandomData(x => x.Email!, 120, 5)
.WithPattern(x => x.Email, @"[a-z]{5,10}@gmail\.com");
//builder.Services
//    .AddRepositoryInTableStorage<User, string>(builder.Configuration["Storage:ConnectionString"]);
//builder.Services
//    .AddRepositoryInCosmosSql<User, string>(
//    x => x.Email!,
//    builder.Configuration["CosmosSql:ConnectionString"],
//    "BigDatabase");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Services.Populate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddApiForRepositoryFramework();

app.Run();