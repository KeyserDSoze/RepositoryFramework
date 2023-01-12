using RepositoryFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add repository framework
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
        /// <param name="services">IServiceCollection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IServiceCollection AddRepository<T, TKey>(this IServiceCollection services,
          Action<IRepositorySettings<T, TKey>> settings)
          where TKey : notnull
        {
            var defaultSettings = new RepositorySettings<T, TKey>(services);
            settings.Invoke(defaultSettings);
            return services;
        }
    }
}
