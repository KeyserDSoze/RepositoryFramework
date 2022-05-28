using RepositoryFramework.Client;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryPatternClient<T, TKey>(this IServiceCollection services,
            string domain,
            string startingPath = "api",
            Action<HttpClient>? configureClient = null,
           ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
           where TKey : notnull
        {
            services.AddHttpClient($"{typeof(T).Name}RepositoryPatternClient", options =>
            {
                configureClient?.Invoke(options);
                options.BaseAddress = new Uri($"https://{domain}/{startingPath}/{typeof(T).Name.ToLower()}/");
            });
            return serviceLifetime switch
            {
                ServiceLifetime.Scoped => services.AddScoped<IRepositoryPatternClient<T, TKey>, RepositoryPatternClient<T, TKey>>(),
                ServiceLifetime.Transient => services.AddTransient<IRepositoryPatternClient<T, TKey>, RepositoryPatternClient<T, TKey>>(),
                ServiceLifetime.Singleton => services.AddSingleton<IRepositoryPatternClient<T, TKey>, RepositoryPatternClient<T, TKey>>(),
                _ => services.AddScoped<IRepositoryPatternClient<T, TKey>, RepositoryPatternClient<T, TKey>>()
            };
        }
    }
}