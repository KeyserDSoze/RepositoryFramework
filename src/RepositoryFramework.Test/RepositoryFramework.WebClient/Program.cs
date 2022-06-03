using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using RepositoryFramework.WebClient.Data;
using RepositoryFramework.WebClient.Interceptors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRepositoryClient<User, string>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryClientInterceptor<Interceptor>();
builder.Services.AddRepositoryClientSpecificInterceptor<User, string, SpecificInterceptor>();
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
