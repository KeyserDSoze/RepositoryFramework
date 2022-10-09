using RepositoryFramework;
using RepositoryFramework.Infrastructure.Dynamics.Dataverse;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default cosmos sql service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your dataverse connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryDataverseBuilder<T, TKey> AddRepositoryDataverse<T, TKey>(
           this IServiceCollection services,
                Action<DataverseOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            options.Invoke(DataverseOptions<T, TKey>.Instance);
            services.AddSingleton(DataverseOptions<T, TKey>.Instance);
            DataverseIntegrations.Instance.Options.Add(DataverseOptions<T, TKey>.Instance);
            return new RepositoryDataverseBuilder<T, TKey>(services.AddRepository<T, TKey, DataverseRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
        /// <summary>
        /// Add a default cosmos sql service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your dataverse connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryDataverseBuilder<T, TKey> AddCommandInDataverse<T, TKey>(
           this IServiceCollection services,
                Action<DataverseOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            options.Invoke(DataverseOptions<T, TKey>.Instance);
            services.AddSingleton(DataverseOptions<T, TKey>.Instance);
            DataverseIntegrations.Instance.Options.Add(DataverseOptions<T, TKey>.Instance);
            return new RepositoryDataverseBuilder<T, TKey>(services.AddCommand<T, TKey, DataverseRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
        /// <summary>
        /// Add a default cosmos sql service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your dataverse connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IRepositoryCosmosSqlBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryDataverseBuilder<T, TKey> AddQueryInDataverse<T, TKey>(
           this IServiceCollection services,
                Action<DataverseOptions<T, TKey>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
        {
            options.Invoke(DataverseOptions<T, TKey>.Instance);
            services.AddSingleton(DataverseOptions<T, TKey>.Instance);
            DataverseIntegrations.Instance.Options.Add(DataverseOptions<T, TKey>.Instance);
            return new RepositoryDataverseBuilder<T, TKey>(services.AddQuery<T, TKey, DataverseRepository<T, TKey>>(ServiceLifetime.Singleton, settings));
        }
    }
}
