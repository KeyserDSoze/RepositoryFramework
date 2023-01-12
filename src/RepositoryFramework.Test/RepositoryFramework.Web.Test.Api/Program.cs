using RepositoryFramework;
using RepositoryFramework.Web.Test.BlazorApp.Models;
using Whistleblowing.Licensing.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiFromRepositoryFramework()
    .WithDefaultCors()
    .WithSwagger()
    .WithDocumentation()
    .WithDescriptiveName("Api");

builder.Services
    .AddRepositoryInMemoryStorage<AppConfiguration, string>()
    .PopulateWithRandomData(x => x.AppDomain, 34, 2);

builder.Services.AddRepositoryInMemoryStorage<AppGroup, string>(null, x =>
{
    x.IsNotExposable = false;
})
    .PopulateWithRandomData(x => x.Id, 24, 2);
builder.Services.AddRepositoryInMemoryStorage<Weather, int>()
    .PopulateWithRandomData(x => x.Id, 5, 2);
builder.Services
    .AddRepositoryInMemoryStorage<AppUser, int>()
    .PopulateWithRandomData(x => x.Id, 67, 2)
    .WithRandomValue(x => x.Groups, async serviceProvider =>
    {
        var repository = serviceProvider.GetService<IRepository<AppGroup, string>>()!;
        return (await repository.ToListAsync().NoContext()).Select(x => new Group()
        {
            Id = x.Key,
            Name = x.Value.Name
        });
    });

builder.Services.AddWarmUp(async serviceProvider =>
{
    var repository = serviceProvider.GetService<IRepository<AppUser, int>>();
    if (repository != null)
    {
        await repository.InsertAsync(23, new AppUser
        {
            Email = "23 default",
            Groups = new(),
            Id = 23,
            Name = "23 default",
            Password = "23 default",
            InternalAppSettings = new InternalAppSettings
            {
                Index = 23,
                Maps = new() { "23" },
                Options = "23 default options"
            },
            Settings = new AppSettings
            {
                Color = "23 default",
                Options = "23 default",
                Maps = new() { "23" }
            }
        }).NoContext();
    }
});

var app = builder.Build();
await app.Services.WarmUpAsync();

app.UseHttpsRedirection();
app
    .UseApiFromRepositoryFramework();

app.Run();
