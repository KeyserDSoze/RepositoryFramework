### [What is Rystem?](https://github.com/KeyserDSoze/RystemV3)

## Api auto-generated
In your web application you have only to add one row after service build.

    app.AddApiForRepositoryFramework();

    public static TEndpointRouteBuilder AddApiForRepositoryFramework<TEndpointRouteBuilder>(
        this TEndpointRouteBuilder app,
        string startingPath = "api",
        AuthorizationForApi? authorizationPolicy = null)
        where TEndpointRouteBuilder : IEndpointRouteBuilder
    
You may add api for each service by

        public static IEndpointRouteBuilder AddApiForRepository<T>(this IEndpointRouteBuilder app,
            string startingPath = "api",
            AuthorizationForApi? authorizationPolicy = null)

### Startup example
In the example below you may find the DI for repository with string key for User model, populated with random data in memory, swagger to test the solution, the population method just after the build and the configuration of your API based on repository framework.

    using RepositoryFramework.WebApi.Models;

    var builder = WebApplication.CreateBuilder(args);
    builder.Services
        .AddRepositoryInMemoryStorage<User>()
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


### Sample of query usage when you use the api directly
All the requests are basic requests, the strangest request is only the query and you must use the Linq query.
You may find some examples down below:

    ƒ => (((ƒ.X == "dasda") AndAlso ƒ.X.Contains("dasda")) AndAlso ((ƒ.E == Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2")) Or (ƒ.Id == 32)))
    ƒ => ((((ƒ.X == "dasda") AndAlso ƒ.Sol) AndAlso ƒ.X.Contains("dasda")) AndAlso ((ƒ.E == Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2")) Or (ƒ.Id == 32)))
    ƒ => (((((ƒ.X == "dasda") AndAlso ƒ.Sol) AndAlso ƒ.X.Contains("dasda")) AndAlso ((ƒ.E == Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))
    ƒ => (ƒ.Type == 2)
    ƒ => (((((ƒ.X == "dasda") AndAlso ƒ.Sol) AndAlso (ƒ.X.Contains("dasda") OrElse ƒ.Sol.Equals(True))) AndAlso ((ƒ.E == Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))
    ƒ => ((((((ƒ.X == "dasda") AndAlso ƒ.Samules.Any(x => (x == "ccccde"))) AndAlso ƒ.Sol) AndAlso (ƒ.X.Contains("dasda") OrElse ƒ.Sol.Equals(True))) AndAlso ((ƒ.E == Guid.Parse("bf46510b-b7e6-4ba2-88da-cef208aa81f2")) Or (ƒ.Id == 32))) AndAlso ((ƒ.Type == 1) OrElse (ƒ.Type == 2)))
    ƒ => (ƒ.ExpirationTime > Convert.ToDateTime("7/6/2022 9:48:56 AM"))
    ƒ => (ƒ.TimeSpan > new TimeSpan(1000 as long))
    ƒ => Not(ƒ.Inside.Inside.A.Equals("dasdad"))