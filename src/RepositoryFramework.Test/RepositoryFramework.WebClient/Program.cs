using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using RepositoryFramework.Test.Domain;
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
var retryPolicy = HttpPolicyExtensions
  .HandleTransientHttpError()
  .Or<TimeoutRejectedException>()
  .RetryAsync(3);

builder.Services
    .AddRepositoryApiClient<User, string>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058")
    .ClientBuilder
        .AddPolicyHandler(retryPolicy);
builder.Services.AddRepositoryApiClient<SuperUser, string>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058");
builder.Services.AddRepositoryApiClient<IperUser, string>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058");
builder.Services.AddRepositoryApiClient<Animal, AnimalKey>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058");
builder.Services.AddRepositoryApiClient<AppUser, AppUserKey>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058");
builder.Services.AddRepositoryApiClient<Car, Guid>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058");
builder.Services.AddRepositoryApiClient<Car2, Range>(serviceLifetime: ServiceLifetime.Scoped)
    .WithHttpClient("localhost:7058");
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
