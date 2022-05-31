using RepositoryFramework.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddRepositoryInMemoryStorageWithStringKey<User>()
//.PopulateWithRandomData(x => x.Email!, 120, 5);
builder.Services
    .AddRepositoryInTableStorage<User, string>(builder.Configuration["Storage:ConnectionString"]);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.Services.Populate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.AddApiForRepositoryFramework();

app.Run();