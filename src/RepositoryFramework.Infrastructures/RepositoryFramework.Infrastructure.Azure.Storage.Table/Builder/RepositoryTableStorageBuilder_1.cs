using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Table
{
    internal class RepositoryTableStorageBuilder<T> : RepositoryTableStorageBuilder<T, string>, IRepositoryTableStorageBuilder<T>
    {
        public new IRepositoryBuilder<T> Builder { get; }
        public RepositoryTableStorageBuilder(IRepositoryBuilder<T> builder) : base(builder)
            => Builder = builder;
        public new IServiceCollection Services => Builder.Services;
        public new PatternType Type => Builder.Type;
        public new ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public new IQueryTranslationBuilder<T, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
        public new IRepositoryTableStorageBuilder<T> WithPartitionKey<TProperty>(Expression<Func<T, TProperty>> property)
            => WithProperty(nameof(WithPartitionKey), property);
        public new IRepositoryTableStorageBuilder<T> WithRowKey<TProperty>(Expression<Func<T, TProperty>> property)
            => WithProperty(nameof(WithRowKey), property);
        public new IRepositoryTableStorageBuilder<T> WithTimestamp(Expression<Func<T, DateTime>> property)
            => WithProperty(nameof(WithTimestamp), property);
        public new IRepositoryTableStorageBuilder<T> WithTimestamp(Expression<Func<T, DateTimeOffset>> property)
            => WithProperty(nameof(WithTimestamp), property);
        public new IRepositoryTableStorageBuilder<T> WithTableStorageKeyReader<TKeyReader>()
            where TKeyReader : class, ITableStorageKeyReader<T>
        {
            Builder.Services
                .AddSingleton<ITableStorageKeyReader<T>, TKeyReader>();
            return this;
        }
        private IRepositoryTableStorageBuilder<T> WithProperty<TProperty>(
           string propertyName,
           Expression<Func<T, TProperty>> property)
        {
            string name = property.Body.ToString().Split('.').Last();
            AddPropertyForTableStorageBaseProperties(propertyName, name);
            return this;
        }
        private void AddPropertyForTableStorageBaseProperties(string propertyName, string? name = null)
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
                if (propertyName == nameof(WithPartitionKey))
                    TableStorageOptions<T>.Instance.PartitionKey = property;
                else if (propertyName == nameof(WithRowKey))
                    TableStorageOptions<T>.Instance.RowKey = property;
                else
                    TableStorageOptions<T>.Instance.Timestamp = property;
            }
            Builder.Services.AddSingleton(TableStorageOptions<T>.Instance);
        }
    }
}