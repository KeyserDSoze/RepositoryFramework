using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal sealed class RepositoryTableStorageBuilder<T, TKey> : IRepositoryTableStorageBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public RepositoryTableStorageBuilder(IRepositoryBuilder<T, TKey> builder)
            => Builder = builder;
        public IServiceCollection Services => Builder.Services;
        public PatternType Type => Builder.Type;
        public ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
        public IRepositoryTableStorageBuilder<T, TKey> WithPartitionKey<TProperty, TKeyProperty>(
            Expression<Func<T, TProperty>> property,
            Expression<Func<TKey, TKeyProperty>> keyProperty)
            => WithProperty(nameof(WithPartitionKey), property, keyProperty);
        public IRepositoryTableStorageBuilder<T, TKey> WithRowKey<TProperty, TKeyProperty>(
            Expression<Func<T, TProperty>> property,
            Expression<Func<TKey, TKeyProperty>> keyProperty)
            => WithProperty(nameof(WithRowKey), property, keyProperty);
        public IRepositoryTableStorageBuilder<T, TKey> WithRowKey<TProperty>(
           Expression<Func<T, TProperty>> property)
           => WithProperty<TProperty, object>(nameof(WithRowKey), property, null);
        public IRepositoryTableStorageBuilder<T, TKey> WithTimestamp(Expression<Func<T, DateTime>> property)
            => WithProperty<DateTime, object>(nameof(WithTimestamp), property, null!);

        public IRepositoryTableStorageBuilder<T, TKey> WithTableStorageKeyReader<TKeyReader>()
            where TKeyReader : class, ITableStorageKeyReader<T, TKey>
        {
            Builder.Services
                .AddSingleton<ITableStorageKeyReader<T, TKey>, TKeyReader>();
            return this;
        }
        private IRepositoryTableStorageBuilder<T, TKey> WithProperty<TProperty, TKeyProperty>(
           string propertyName,
           Expression<Func<T, TProperty>> property,
           Expression<Func<TKey, TKeyProperty>>? keyProperty)
        {
            AddPropertyForTableStorageBaseProperties(propertyName, property, keyProperty);
            return this;
        }
        private void AddPropertyForTableStorageBaseProperties<TProperty, TKeyProperty>(string propertyName,
            Expression<Func<T, TProperty>>? property,
            Expression<Func<TKey, TKeyProperty>>? keyProperty)
        {
            if (property == null)
            {
                TableStorageOptions<T, TKey>.Instance.PartitionKeyFunction = x => typeof(T).FetchProperties().First().GetValue(x)!.ToString()!;
                TableStorageOptions<T, TKey>.Instance.RowKeyFunction = x => typeof(T).FetchProperties().Skip(1).First().GetValue(x)!.ToString()!;
                TableStorageOptions<T, TKey>.Instance.TimestampFunction = x => (DateTime)(typeof(T).FetchProperties().FirstOrDefault(x => x.PropertyType == typeof(DateTime))?.GetValue(x) ?? DateTime.MinValue);
                TableStorageOptions<T, TKey>.Instance.PartitionKey = typeof(T).FetchProperties().First().Name;
                TableStorageOptions<T, TKey>.Instance.RowKey = typeof(T).FetchProperties().Skip(1).First().Name;
                TableStorageOptions<T, TKey>.Instance.Timestamp = typeof(T).FetchProperties().FirstOrDefault(x => x.PropertyType == typeof(DateTime))?.Name;
            }
            else
            {
                string name = property.Body.ToString().Split('.').Last();
                var compiledProperty = property.Compile();
                var compiledKeyProperty = keyProperty?.Compile();
                if (propertyName == nameof(WithPartitionKey))
                {
                    TableStorageOptions<T, TKey>.Instance.PartitionKeyFunction = x => compiledProperty(x)!.ToString()!;
                    TableStorageOptions<T, TKey>.Instance.PartitionKey = name;
                    if (compiledKeyProperty != null)
                        TableStorageOptions<T, TKey>.Instance.PartitionKeyFromKeyFunction = x => compiledKeyProperty(x)!.ToString()!;
                }
                else if (propertyName == nameof(WithRowKey))
                {
                    TableStorageOptions<T, TKey>.Instance.RowKeyFunction = x => compiledProperty(x)!.ToString()!;
                    if (compiledKeyProperty != null)
                    {
                        TableStorageOptions<T, TKey>.Instance.RowKey = name;
                        TableStorageOptions<T, TKey>.Instance.RowKeyFromKeyFunction = x => compiledKeyProperty(x)!.ToString()!;
                    }
                }
                else
                {
                    TableStorageOptions<T, TKey>.Instance.TimestampFunction = x => Convert.ToDateTime(compiledProperty(x)!);
                    TableStorageOptions<T, TKey>.Instance.Timestamp = name;
                }
            }
            Builder.Services.AddSingleton(TableStorageOptions<T, TKey>.Instance);
        }
    }
}
