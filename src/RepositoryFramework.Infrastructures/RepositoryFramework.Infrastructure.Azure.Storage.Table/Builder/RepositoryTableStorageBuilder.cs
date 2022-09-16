using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

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
        public IRepositoryTableStorageBuilder<T, TKey> WithPartitionKey<TProperty>(Expression<Func<T, TProperty>> property)
            => WithProperty(nameof(WithPartitionKey), property);
        public IRepositoryTableStorageBuilder<T, TKey> WithRowKey<TProperty>(Expression<Func<T, TProperty>> property)
            => WithProperty(nameof(WithRowKey), property);

        public IRepositoryTableStorageBuilder<T, TKey> WithTimestamp(Expression<Func<T, DateTime>> property)
            => WithProperty(nameof(WithTimestamp), property);

        public IRepositoryTableStorageBuilder<T, TKey> WithTimestamp(Expression<Func<T, DateTimeOffset>> property)
            => WithProperty(nameof(WithTimestamp), property);

        public IRepositoryTableStorageBuilder<T, TKey> WithTableStorageKeyReader<TKeyReader>()
            where TKeyReader : class, ITableStorageKeyReader<T, TKey>
        {
            Builder.Services
                .AddSingleton<ITableStorageKeyReader<T, TKey>, TKeyReader>();
            return this;
        }
        private IRepositoryTableStorageBuilder<T, TKey> WithProperty<TProperty>(
           string propertyName,
           Expression<Func<T, TProperty>> property)
        {
            var name = property.Body.ToString().Split('.').Last();
            AddPropertyForTableStorageBaseProperties(propertyName, name);
            return this;
        }
        private void AddPropertyForTableStorageBaseProperties(string propertyName, string? name = null)
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
                if (propertyName == nameof(WithPartitionKey))
                    TableStorageOptions<T, TKey>.Instance.PartitionKey = property;
                else if (propertyName == nameof(WithRowKey))
                    TableStorageOptions<T, TKey>.Instance.RowKey = property;
                else
                    TableStorageOptions<T, TKey>.Instance.Timestamp = property;
            }
            Builder.Services.AddSingleton(TableStorageOptions<T, TKey>.Instance);
        }
    }
}
