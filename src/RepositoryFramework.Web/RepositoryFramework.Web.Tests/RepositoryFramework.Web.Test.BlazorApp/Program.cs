using RepositoryFramework.Web.Test.BlazorApp.Data;
using RepositoryFramework.Web.Test.BlazorApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services
    .AddRepositoryUI(x => x.Name = "SuperSite");
builder.Services.AddRepositoryInMemoryStorage<AppUser, int>()
    .PopulateWithRandomData(x => x.Id, 67, 2);
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
    x.AddDefaultRepositoryEndpoint();
});

app.MapFallbackToPage("/_Host");

app.Run();
