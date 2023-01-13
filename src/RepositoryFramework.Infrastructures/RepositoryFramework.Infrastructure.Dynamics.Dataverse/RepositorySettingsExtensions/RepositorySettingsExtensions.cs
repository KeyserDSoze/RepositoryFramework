using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using RepositoryFramework;
using RepositoryFramework.Infrastructure.Dynamics.Dataverse;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositorySettingsExtensions
    {
        private static IRepositoryDataverseBuilder<T, TKey> WithDataverse<T, TKey>(
          this RepositorySettings<T, TKey> settings,
            PatternType type,
            Action<DataverseOptions<T, TKey>> options)
            where TKey : notnull
        {
            options.Invoke(DataverseOptions<T, TKey>.Instance);
            settings.Services.AddSingleton(DataverseOptions<T, TKey>.Instance);
            settings.Services.AddEventAfterServiceCollectionBuild(serviceProvider => DataverseCreateTableOrMergeNewColumnsInExistingTableAsync(DataverseOptions<T, TKey>.Instance));
            settings.SetStorage<DataverseRepository<T, TKey>>(type, ServiceLifetime.Singleton);
            return new RepositoryDataverseBuilder<T, TKey>(settings.Services);
        }
        /// <summary>
        /// Add a default cosmos sql service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your dataverse connection.</param>
        /// <returns>IRepositoryDataverseBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryDataverseBuilder<T, TKey> WithDataverse<T, TKey>(
           this RepositorySettings<T, TKey> settings,
            Action<DataverseOptions<T, TKey>> options)
            where TKey : notnull
            => settings.WithDataverse(PatternType.Repository, options);
        /// <summary>
        /// Add a default cosmos sql service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your dataverse connection.</param>
        /// <returns>IRepositoryDataverseBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryDataverseBuilder<T, TKey> WithCommandInDataverse<T, TKey>(
           this RepositorySettings<T, TKey> settings,
            Action<DataverseOptions<T, TKey>> options)
            where TKey : notnull
            => settings.WithDataverse(PatternType.Command, options);
        /// <summary>
        /// Add a default cosmos sql service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your dataverse connection.</param>
        /// <returns>IRepositoryDataverseBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositoryDataverseBuilder<T, TKey> WithQueryInDataverse<T, TKey>(
           this RepositorySettings<T, TKey> settings,
            Action<DataverseOptions<T, TKey>> options)
            where TKey : notnull
            => settings.WithDataverse(PatternType.Query, options);
    }
}
