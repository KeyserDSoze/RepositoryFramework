using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    internal sealed class RepositoryDataverseBuilder<T, TKey> : IRepositoryDataverseBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public RepositoryDataverseBuilder(IRepositoryBuilder<T, TKey> builder)
            => Builder = builder;
        public IServiceCollection Services => Builder.Services;
        public PatternType Type => Builder.Type;
        public ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
        public IRepositoryDataverseBuilder<T, TKey> WithKeyManager<TKeyReader>()
            where TKeyReader : class, IDataverseKeyManager<T, TKey>
        {
            Builder.Services
                .AddSingleton<IDataverseKeyManager<T, TKey>, TKeyReader>();
            return this;
        }
        public IRepositoryDataverseBuilder<T, TKey> WithId(Expression<Func<T, TKey>> property)
        {
            var compiled = property.Compile();
            Builder.Services
                .AddSingleton<IDataverseKeyManager<T, TKey>>(
                new DefaultDataverseKeyManager<T, TKey>(x => compiled.Invoke(x)));
            return this;
        }
    }
}
