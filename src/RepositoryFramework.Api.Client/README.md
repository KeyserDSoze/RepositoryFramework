## Services extensions
You may add a repository client for your model. You may choose the domain (domain where the api is), and the custom path by default is "api", you may add custom configuration to the HttpClient and the service lifetime with singleton as default. The api url will be https://{domain}/{startingPath}/{ModelName}/{Type of Api (from Insert, Update, Delete, Get, Query, Exist)}

    public static IServiceCollection AddRepositoryApiClient<T, TKey>(this IServiceCollection services,
        string domain,
        string startingPath = "api",
        Action<HttpClient>? configureClient = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TKey : notnull
        {

        }

You have the same client for CQRS, with command
    
     public static IServiceCollection AddCommandApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull

and query
    
     public static IServiceCollection AddQueryApiClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull

### HttpClient to use your API (example)
You can add a client for a specific url

    services.AddRepositoryApiClient<User, string>("localhost:7058");
    
and use it in DI with
    
    IRepository<User, string> repository

### Query and Command
In DI you install the services

    services.AddCommandApiClient<User, string>("localhost:7058");
    services.AddQueryApiClient<User, string>("localhost:7058");

And you may inject the objects
    
    ICommand<User, string> command
    IQuery<User, string> command

### With TState
In DI you install the services, We're using a class Result as TState.

    services.AddRepositoryApiClient<User, string, Result>("localhost:7058");
    services.AddCommandApiClient<User, string, Result>("localhost:7058");
    services.AddQueryApiClient<User, string, Result>("localhost:7058");

And you may inject the objects
    
    IRepository<User, string, Result> repository
    ICommand<User, string, Result> command
    IQuery<User, string, Result> command

### With string as default TKey 
In DI you install the services

    services.AddRepositoryApiClient<User>("localhost:7058");
    services.AddCommandApiClient<User>("localhost:7058");
    services.AddQueryApiClient<User>("localhost:7058");

And you may inject the objects
    
    IRepository<User> repository
    ICommand<User> command
    IQuery<User> command

### Interceptors
You may add a custom interceptor for every request

    public static IServiceCollection AddRepositoryApiClientInterceptor<TInterceptor>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TInterceptor : class, IRepositoryClientInterceptor

or a specific interceptor for every model
    
    public static IServiceCollection AddRepositoryApiClientSpecificInterceptor<T, TInterceptor>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TInterceptor : class, IRepositoryClientInterceptor<T>
        where TKey : notnull

Maybe you can use it to add a token as JWT o another pre-request things.