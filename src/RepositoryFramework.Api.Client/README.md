## Services extensions
You may add a repository client for your model. You may choose the domain (domain where the api is), and the custom path by default is "api", you may add custom configuration to the HttpClient and the service lifetime with singleton as default. The api url will be https://{domain}/{startingPath}/{ModelName}/{Type of Api (from Insert, Update, Delete, Get, Query, Exist)}

    public static IServiceCollection AddRepositoryClient<T, TKey>(this IServiceCollection services,
        string domain,
        string startingPath = "api",
        Action<HttpClient>? configureClient = null,
        ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TKey : notnull
        {

        }

You have the same client for CQRS, with command
    
     public static IServiceCollection AddCommandClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull

and query
    
     public static IServiceCollection AddQueryClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
           where TKey : notnull

### HttpClient to use your API (example)
You can add a client for a specific url

    .AddRepositoryClient<User, string>("localhost:7058");
    
and use it in DI with
    
    IRepositoryClient<User, string>

### Interceptors
You may add a custom interceptor for every request

    public static IServiceCollection AddRepositoryClientInterceptor<TInterceptor>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TInterceptor : class, IRepositoryClientInterceptor

or a specific interceptor for every model
    
    public static IServiceCollection AddRepositoryClientSpecificInterceptor<T, TKey, TInterceptor>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TInterceptor : class, IRepositoryClientInterceptor<T, TKey>
        where TKey : notnull

or a specific interceptor for every model with a object type key (default key)
    
    public static IServiceCollection AddRepositoryClientSpecificInterceptor<T, TInterceptor>(this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TInterceptor : class, IRepositoryClientInterceptor<T, object>