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
        /// <typeparam name="TKeyParser">Implementation of your key reader.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithTableStorageReader<T, TParser>(
           this IRepositoryBuilder<T> builder)
            where TParser : class, ITableStorageEntityReader<T>
        {
            builder.Services
                .AddSingleton<ITableStorageEntityReader<T>, TParser>();
            return builder;
        }

        /// <summary>
        /// Map PartitionKey on a specific property of your model.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="property">Partition key property.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithPartitionKey<T>(
           this IRepositoryBuilder<T> builder,
           Expression<Func<T, object>> property)
            => builder.WithSomeProperty(TableStoragePropertyType.PartitionKey, property);
        /// <summary>
        /// Map RowKey on a specific property of your model.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="property">Row key property.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithRowKey<T>(
           this IRepositoryBuilder<T> builder,
           Expression<Func<T, object>> property)
            => builder.WithSomeProperty(TableStoragePropertyType.RowKey, property);
        /// <summary>
        /// Map Timestamp on a specific property of your model.
        /// </summary>
        /// <typeparam name="T">Model used for your repository.</typeparam>
        /// <param name="builder">RepositoryBuilder<<typeparamref name="T"/>></param>
        /// <param name="property">Timestamp property.</param>
        /// <returns>IRepositoryBuilder<<typeparamref name="T"/>></returns>
        public static IRepositoryBuilder<T> WithTimestamp<T>(
           this IRepositoryBuilder<T> builder,
           Expression<Func<T, DateTime>> property)
            => builder.WithSomeProperty(TableStoragePropertyType.Timestamp, property);
        internal static IRepositoryBuilder<T> WithSomeProperty<T, TProperty>(
           this IRepositoryBuilder<T> builder,
           TableStoragePropertyType propertyType,
           Expression<Func<T, TProperty>> property)
        {
            string name = property.Body.ToString().Split('.').Last();
            return builder.WithSomething(propertyType, name);
        }
        internal static IRepositoryBuilder<T> WithSomething<T>(
           this IRepositoryBuilder<T> builder,
           TableStoragePropertyType propertyType,
           string? name = null)
        {
            if (name == null)
            {
                TableStorageOptions<T>.Instance.PartitionKey = typeof(T).FetchProperties().First();
                TableStorageOptions<T>.Instance.RowKey = typeof(T).FetchProperties().Skip(1).First();
                TableStorageOptions<T>.Instance.Timestamp = typeof(T).FetchProperties()
                    .FirstOrDefault(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTimeOffset));
            }
            else
            {
                var property = typeof(T).FetchProperties().First(x => x.Name == name);
                if (propertyType == TableStoragePropertyType.PartitionKey)
                    TableStorageOptions<T>.Instance.PartitionKey = property;
                else if (propertyType == TableStoragePropertyType.RowKey)
                    TableStorageOptions<T>.Instance.RowKey = property;
                else
                    TableStorageOptions<T>.Instance.Timestamp = property;
            }
            builder.Services.AddSingleton(TableStorageOptions<T>.Instance);
            return builder;
        }
    }
}
