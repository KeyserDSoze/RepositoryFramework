using System.Data;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.MsSql
{
    internal sealed class RepositoryMsSqlBuilder<T, TKey> : IRepositoryMsSqlBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public RepositoryMsSqlBuilder(IRepositoryBuilder<T, TKey> builder)
            => Builder = builder;
        public IServiceCollection Services => Builder.Services;
        public PatternType Type => Builder.Type;
        public ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
        public IRepositoryMsSqlBuilder<T, TKey> WithPrimaryKey<TProperty>(Expression<Func<T, TProperty>> property, Action<PropertyHelper<T>> value)
        {
            var propertyName = property.Body.ToString().Split('.').Last();
            var prop = MsSqlOptions<T, TKey>.Instance.Properties.First(x => x.PropertyInfo.Name == propertyName);
            value.Invoke(prop);
            MsSqlOptions<T, TKey>.Instance.PrimaryKey = prop.ColumnName;
            MsSqlOptions<T, TKey>.Instance.RefreshColumnNames();
            return this;
        }
        public IRepositoryMsSqlBuilder<T, TKey> WithColumn<TProperty>(Expression<Func<T, TProperty>> property, Action<PropertyHelper<T>> value)
        {
            var propertyName = property.Body.ToString().Split('.').Last();
            var prop = MsSqlOptions<T, TKey>.Instance.Properties.First(x => x.PropertyInfo.Name == propertyName);
            value.Invoke(prop);
            MsSqlOptions<T, TKey>.Instance.RefreshColumnNames();
            return this;
        }
    }
}
