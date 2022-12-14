using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using RepositoryFramework.Web.Test.BlazorApp.Models;
using Whistleblowing.Licensing.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services
        .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(builder.Configuration["AzureAd:Scopes"]!.Split(' '))
        .AddInMemoryTokenCaches();
builder.Services.AddAuthorization();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
builder.Services
    .AddRepositoryUi<ApiConfiguration>(x =>
    {
        x.Name = "SuperSite";
        x.Icon = "savings";
        x.Image = "https://www.pngitem.com/pimgs/m/432-4328680_crime-dashboard-navigation-icon-emblem-hd-png-download.png";
    })
    .WithAuthenticatedUi();
builder.Services.AddApplicationInsightsTelemetry(x =>
{
    x.ConnectionString = "in secrets";
});
builder.Services
    .AddRepositoryInMemoryStorage<AppConfiguration, string>()
    .PopulateWithRandomData(x => x.AppDomain, 34, 2)
    .And()
    .ExposeFor(3);
builder.Services
    .AddRepositoryInMemoryStorage<AppUser, int>()
    .PopulateWithRandomData(x => x.Id, 67, 2)
    .And()
    .MapPropertiesForUi<AppUser, int, AppUserDesignMapper>()
    .WithIcon("manage_accounts")
    .WithName("User")
    .ExposeFor(2);
builder.Services.AddRepositoryInMemoryStorage<AppGroup, string>(null, x =>
{
    x.IsNotExposable = false;
})
    .PopulateWithRandomData(x => x.Id, 24, 2);
builder.Services.AddRepositoryInMemoryStorage<Weather, int>()
    .PopulateWithRandomData(x => x.Id, 5, 2)
    .And()
    .SetDefaultUiRoot();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.AddDefaultRepositoryEndpoints();

app.Run();
