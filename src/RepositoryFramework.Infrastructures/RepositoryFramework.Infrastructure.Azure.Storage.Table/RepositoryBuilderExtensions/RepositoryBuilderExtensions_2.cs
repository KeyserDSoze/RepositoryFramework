using RepositoryFramework;
using RepositoryFramework.Infrastructure.Azure.Storage.Table;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class RepositoryBuilderExtensions
    {
        /// <summary>
        /// Add a custom key reader to your tablestorage integration. To read correctly the Partition and Row key.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <typeparam name="TParser">Implementation of your table storage reader.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>,<typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithTableStorageReader<T, TKey, TParser>(
           this IRepositoryBuilder<T, TKey> builder)
            where TKey : notnull
            where TParser : class, ITableStorageEntityReader<T, TKey>
        {
            builder.Services
                .AddSingleton<ITableStorageEntityReader<T, TKey>, TParser>();
            return builder;
        }
        /// <summary>
        /// Map PartitionKey on a specific property of your model.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="property">Partition key property.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>,<typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithPartitionKey<T, TKey>(
           this IRepositoryBuilder<T, TKey> builder,
           Expression<Func<T, object>> property)
            where TKey : notnull
            => builder.WithSomeProperty(TableStoragePropertyType.PartitionKey, property);
        /// <summary>
        /// Map RowKey on a specific property of your model.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="property">Row key property.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>,<typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithRowKey<T, TKey>(
           this IRepositoryBuilder<T, TKey> builder,
           Expression<Func<T, object>> property)
            where TKey : notnull
            => builder.WithSomeProperty(TableStoragePropertyType.RowKey, property);
        /// <summary>
        /// Map Timestamp on a specific property of your model.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <typeparam name="TKey">Key to manage your data from repository</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="property">Timestamp property.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>,<typeparamref name="TKey"/>></returns>
        public static IRepositoryBuilder<T, TKey> WithTimestamp<T, TKey>(
           this IRepositoryBuilder<T, TKey> builder,
           Expression<Func<T, DateTime>> property)
            where TKey : notnull 
            => builder.WithSomeProperty(TableStoragePropertyType.Timestamp, property);
        internal static IRepositoryBuilder<T, TKey> WithSomeProperty<T, TKey, TProperty>(
           this IRepositoryBuilder<T, TKey> builder,
           TableStoragePropertyType propertyType,
           Expression<Func<T, TProperty>> property)
            where TKey : notnull
        {
            string name = property.Body.ToString().Split('.').Last();
            return builder.WithSomething(propertyType, name);
        }
        internal static IRepositoryBuilder<T, TKey> WithSomething<T, TKey>(
           this IRepositoryBuilder<T, TKey> builder,
           TableStoragePropertyType propertyType,
           string? name = null)
            where TKey : notnull
        {
            if (name == null)
            {
                TableStorageOptions<T, TKey>.Instance.PartitionKey = typeof(T).FetchProperties().First();
                TableStorageOptions<T, TKey>.Instance.RowKey = typeof(T).FetchProperties().Skip(1).First();
                TableStorageOptions<T, TKey>.Instance.Timestamp = typeof(T).FetchProperties()
                    .FirstOrDefault(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTimeOffset));
            }
            else
            {
                var property = typeof(T).FetchProperties().First(x => x.Name == name);
                if (propertyType == TableStoragePropertyType.PartitionKey)
                    TableStorageOptions<T, TKey>.Instance.PartitionKey = property;
                else if (propertyType == TableStoragePropertyType.RowKey)
                    TableStorageOptions<T, TKey>.Instance.RowKey = property;
                else
                    TableStorageOptions<T, TKey>.Instance.Timestamp = property;
            }
            builder.Services.AddSingleton(TableStorageOptions<T, TKey>.Instance);
            return builder;
        }
    }
}
