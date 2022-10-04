using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    internal sealed class RepositoryCosmosSqlBuilder<T, TKey> : IRepositoryCosmosSqlBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public RepositoryCosmosSqlBuilder(IRepositoryBuilder<T, TKey> builder)
            => Builder = builder;
        public IServiceCollection Services => Builder.Services;
        public PatternType Type => Builder.Type;
        public ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
        public IRepositoryCosmosSqlBuilder<T, TKey> WithKeyManager<TKeyReader>()
            where TKeyReader : class, ICosmosSqlKeyManager<T, TKey>
        {
            Builder.Services
                .AddSingleton<ICosmosSqlKeyManager<T, TKey>, TKeyReader>();
            return this;
        }
        public IRepositoryCosmosSqlBuilder<T, TKey> WithId(Expression<Func<T, TKey>> property)
        {
            var compiled = property.Compile();
            Builder.Services
                .AddSingleton<ICosmosSqlKeyManager<T, TKey>>(
                new DefaultCosmosSqlKeyManager<T, TKey>(x => compiled.Invoke(x)));
            return this;
        }
    }
}
