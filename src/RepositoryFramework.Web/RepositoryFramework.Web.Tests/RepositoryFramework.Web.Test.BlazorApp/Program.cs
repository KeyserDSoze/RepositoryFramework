using RepositoryFramework.Web.Components;
using RepositoryFramework.Web.Test.BlazorApp.Models;
using Whistleblowing.Licensing.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddServerSideBlazor();
builder.Services
    .AddRepositoryUi(x =>
    {
        x.Name = "SuperSite";
        x.Palette = AppPalette.Pastels;
    });

builder.Services
    .AddRepositoryInMemoryStorage<AppConfiguration, string>()
    .PopulateWithRandomData(x => x.AppDomain, 34, 2)
    .And()
    .SetDefaultUiRoot();
builder.Services
    .AddRepositoryInMemoryStorage<AppUser, int>()
    .PopulateWithRandomData(x => x.Id, 67, 2)
    .And()
    .MapPropertiesForUi<AppUser, int, AppUserDesignMapper>();
builder.Services.AddRepositoryInMemoryStorage<AppGroup, string>(null, x =>
{
    x.IsNotExposable = false;
})
    .PopulateWithRandomData(x => x.Id, 24, 2);
builder.Services.AddRepositoryInMemoryStorage<Weather, int>()
    .PopulateWithRandomData(x => x.Id, 5, 2);
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
app.AddDefaultRepositoryEndpoints();

app.Run();
