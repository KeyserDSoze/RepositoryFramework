using Microsoft.EntityFrameworkCore;
using RepositoryFramework;
using RepositoryFramework.Infrastructure.EntityFramework;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add a default Entity Framework service for your repository pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IQueryTranslationBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="T"/>></returns>
        public static IQueryTranslationBuilder<T, TKey, T> AddRepositoryInEntityFramework<T, TKey, TContext>(
           this IServiceCollection services,
                Action<EntityFrameworkOptions<T, TKey, T, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = services.AddRepositoryInEntityFramework<T, TKey, T, TContext>(options, settings);
            return builder.Translate<T>().WithSamePorpertiesName();
        }
        /// <summary>
        /// Add a default Entity Framework service for your command pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IQueryTranslationBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="T"/>></returns>
        public static IQueryTranslationBuilder<T, TKey, T> AddCommandInEntityFramework<T, TKey, TContext>(
           this IServiceCollection services,
                  Action<EntityFrameworkOptions<T, TKey, T, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = services.AddCommandInEntityFramework<T, TKey, T, TContext>(options, settings);
            return builder.Translate<T>().WithSamePorpertiesName();
        }
        /// <summary>
        /// Add a default Entity Framework service for your query pattern.
        /// </summary>
        /// <typeparam name="T">Model used for your repository</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TContext">Specify DB context to use. Please remember to configure it in DI.</typeparam>
        /// <param name="services">IServiceCollection</param>
        /// <param name="options">Settings for your Entity Framework connection.</param>
        /// <param name="settings">Settings for your repository.</param>
        /// <returns>IQueryTranslationBuilder<<typeparamref name="T"/>, <typeparamref name="TKey"/>, <typeparamref name="T"/>></returns>
        public static IQueryTranslationBuilder<T, TKey, T> AddQueryInEntityFramework<T, TKey, TContext>(
           this IServiceCollection services,
                 Action<EntityFrameworkOptions<T, TKey, T, TContext>> options,
                Action<RepositoryFrameworkOptions<T, TKey>>? settings = null)
            where TKey : notnull
            where T : class
            where TContext : DbContext
        {
            var builder = services.AddQueryInEntityFramework<T, TKey, T, TContext>(options, settings);
            return builder.Translate<T>().WithSamePorpertiesName();
        }
    }
}
