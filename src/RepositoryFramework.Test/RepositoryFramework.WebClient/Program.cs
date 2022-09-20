using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using RepositoryFramework.WebClient.Data;

var builder = WebApplication.CreateBuilder(args);

var scopes = builder.Configuration["AzureAd:Scopes"];
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
               .EnableTokenAcquisitionToCallDownstreamApi(scopes.Split(' '))
               .AddInMemoryTokenCaches();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRepositoryApiClient<User, string>("localhost:7058",
    serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClient<SuperUser, string>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClient<IperUser, string>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClient<Animal, AnimalKey>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClient<Car, Guid>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClient<Car2, Range>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddDefaultAuthorizationInterceptorForApiHttpClient(settings =>
{
    settings.Scopes = builder.Configuration["AzureAd:Scopes"].Split(' ');
});

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

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
app.MapFallbackToPage("/_Host");

app.Run();
