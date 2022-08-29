using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Storage.Table;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add a custom key reader to your tablestorage integration. To read correctly the Partition and Row key.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKeyParser">Implementation of your key reader.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithKeyReader<T, TKeyParser>(
           this IRepositoryBuilder<T> builder)
            where TKeyParser : class, ITableStorageKeyReader<T>
        {
            builder.Services
                .AddSingleton<ITableStorageKeyReader<T>, TKeyParser>();
            return builder;
        }
        /// <summary>
        /// Add a custom key reader to your tablestorage integration. To read correctly the Partition and Row key.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TKeyParser">Implementation of your key reader.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>,<typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithKeyReader<T, TKey, TKeyParser>(
           this IRepositoryBuilder<T, TKey> builder)
            where TKey : notnull
            where TKeyParser : class, ITableStorageKeyReader<T, TKey>
        {
            builder.Services
                .AddSingleton<ITableStorageKeyReader<T, TKey>, TKeyParser>();
            return builder;
        }
    }
}
