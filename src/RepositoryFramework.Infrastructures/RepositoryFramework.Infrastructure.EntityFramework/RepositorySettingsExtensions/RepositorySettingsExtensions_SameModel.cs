using Microsoft.EntityFrameworkCore;
using RepositoryFramework;
using RepositoryFramework.Infrastructure.EntityFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositorySettingsExtensions
    {
        /// <summary>
        /// Add a default Entity Framework service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <returns>IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositorySettings<T, TKey> WithEntityFramework<T, TKey, TContext>(
           this IRepositorySettings<T, TKey> settings,
                Action<EntityFrameworkOptions<T, TKey, T, TContext>> options)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = settings.WithEntityFramework(PatternType.Repository, options);
            builder.Translate<T>().WithSamePorpertiesName();
            return settings;
        }
        /// <summary>
        /// Add a default Entity Framework service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <returns>IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositorySettings<T, TKey> WithCommandInEntityFramework<T, TKey, TContext>(
           this IRepositorySettings<T, TKey> settings,
                  Action<EntityFrameworkOptions<T, TKey, T, TContext>> options)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = settings.WithEntityFramework(PatternType.Command, options);
            builder.Translate<T>().WithSamePorpertiesName();
            return settings;
        }
        /// <summary>
        /// Add a default Entity Framework service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <returns>IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></returns>
        public static IRepositorySettings<T, TKey> WithQueryInEntityFramework<T, TKey, TContext>(
           this IRepositorySettings<T, TKey> settings,
                 Action<EntityFrameworkOptions<T, TKey, T, TContext>> options)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = settings.WithEntityFramework(PatternType.Query, options);
            builder.Translate<T>().WithSamePorpertiesName();
            return settings;
        }
    }
}
