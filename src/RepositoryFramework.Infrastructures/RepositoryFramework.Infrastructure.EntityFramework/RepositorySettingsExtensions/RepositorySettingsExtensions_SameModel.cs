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
        /// <returns>IQueryTranslationBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, IRepository<<typeparamref name="T"/>, <typeparamref name="TKey"/>>, <typeparamref name="T"/>>></returns>
        public static IQueryTranslationBuilder<T, TKey, IRepository<T, TKey>, T> WithEntityFramework<T, TKey, TContext>(
           this IRepositorySettings<T, TKey> settings,
                Action<EntityFrameworkOptions<T, TKey, T, TContext>> options)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = settings.WithEntityFramework(PatternType.Repository, options);
            return builder.Translate<T>().WithSamePorpertiesName();
        }
        /// <summary>
        /// Add a default Entity Framework service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <returns>IQueryTranslationBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, ICommand<<typeparamref name="T"/>, <typeparamref name="TKey"/>>, <typeparamref name="T"/>>></returns>
        public static IQueryTranslationBuilder<T, TKey, ICommand<T, TKey>, T> WithCommandInEntityFramework<T, TKey, TContext>(
           this IRepositorySettings<T, TKey> settings,
                  Action<EntityFrameworkOptions<T, TKey, T, TContext>> options)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = settings.WithEntityFramework(PatternType.Command, options);
            return builder.Translate<T>().WithSamePorpertiesName();
        }
        /// <summary>
        /// Add a default Entity Framework service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="settings">IRepositorySettings<<typeparamref name="T"/>, <typeparamref name="TKey"/>></param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <returns>IQueryTranslationBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, IQuery<<typeparamref name="T"/>, <typeparamref name="TKey"/>>, <typeparamref name="T"/>>></returns>
        public static IQueryTranslationBuilder<T, TKey, IQuery<T, TKey>, T> WithQueryInEntityFramework<T, TKey, TContext>(
           this IRepositorySettings<T, TKey> settings,
                 Action<EntityFrameworkOptions<T, TKey, T, TContext>> options)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = settings.WithEntityFramework(PatternType.Query, options);
            return builder.Translate<T>().WithSamePorpertiesName();
        }
    }
}
