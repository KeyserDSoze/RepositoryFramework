using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework;
using RepositoryFramework.Web.Components;
using RepositoryFramework.Web.Test.BlazorApp.Data;
using RepositoryFramework.Web.Test.BlazorApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServerSideBlazor();
builder.Services
    .AddRepositoryUi(x =>
    {
        x.Name = "SuperSite";
        x.Palette = AppPalette.Pastels;
    })
    .WithDefault<AppUser>()
    .Configure<AppUser, int>()
    .MapDefault(x => x.Email, "Default email")
    .MapChoices(x => x.Groups, async (serviceProvider) =>
    {
        var repository = serviceProvider.GetService<IRepository<AppGroup, string>>();
        List<PropertyValue> values = new();
        await foreach (var entity in repository.QueryAsync())
            values.Add(new PropertyValue
            {
                Label = entity.Value.Name,
                Value = new Group
                {
                    Id = entity.Value.Id,
                    Name = entity.Value.Name,
                }
            });
        return values;
    }, x => x.Name)
    .AndConfigure<AppGroup, int>()
    .MapChoice(x => x.Name, serviceProvider =>
    {
        return Task.FromResult(new List<PropertyValue> {
            "Admin",
            "SuperAdmin",
            "AppManager",
            "SuperAppManager" }.AsEnumerable());
    }, x => x);

builder.Services.AddRepositoryInMemoryStorage<AppUser, int>()
    .PopulateWithRandomData(x => x.Id, 67, 2);
builder.Services.AddRepositoryInMemoryStorage<AppGroup, string>(null, x =>
{
    x.IsNotExposable = false;
})
    .PopulateWithRandomData(x => x.Id, 24, 2);
var app = builder.Build();
await app.Services.WarmUpAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.UseEndpoints(x =>
{
    x.AddDefaultRepositoryEndpoints();
});

app.MapFallbackToPage("/_Host");

app.Run();
