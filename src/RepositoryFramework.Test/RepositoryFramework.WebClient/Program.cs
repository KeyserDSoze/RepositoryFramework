using RepositoryFramework.WebClient.Data;
using RepositoryFramework.WebClient.Interceptors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddRepositoryApiClient<User>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped)
        .AddRepositoryApiClientSpecificInterceptor<User, string, bool, SpecificInterceptor>();
builder.Services.AddRepositoryApiClient<SuperUser, string>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClient<IperUser, string, bool>("localhost:7058", serviceLifetime: ServiceLifetime.Scoped);
builder.Services.AddRepositoryApiClientInterceptor<Interceptor>();
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
