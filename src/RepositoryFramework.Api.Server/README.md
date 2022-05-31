## Api auto-generated
In your web application you have only to add one row after service build.

    app.AddApiForRepositoryFramework();

    public static TEndpointRouteBuilder AddApiForRepositoryFramework<TEndpointRouteBuilder>(
        this TEndpointRouteBuilder app,
        string startingPath = "api",
        AuthorizationForApi? authorizationPolicy = null)
    
You may add api for each service by

        public static IEndpointRouteBuilder AddApiForRepository<T>(this IEndpointRouteBuilder app,
            string startingPath = "api",
            AuthorizationForApi? authorizationPolicy = null)

### Startup example
In the example below you may find the DI for repository with string key for User model, populated with random data in memory, swagger to test the solution, the population method just after the build and the configuration of your API based on repository framework.

    using RepositoryFramework.WebApi.Models;

    var builder = WebApplication.CreateBuilder(args);
    builder.Services
        .AddRepositoryInMemoryStorageWithStringKey<User>()
        .PopulateWithRandomData(x => x.Email!, 120, 5);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    
    var app = builder.Build();
    app.Services.Populate();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.AddApiForRepositoryFramework();
    app.Run();
