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
        public IRepositoryDataverseBuilder<T, TKey> WithColumn<TProperty>(Expression<Func<T, TProperty>> property,
            string? customPrefix = null)
        {
            var name = property.Body.ToString().Split('.').Last();
            var prop = DataverseOptions<T, TKey>.Instance.Properties.First(x => x.Name == name);
            prop.Prefix = customPrefix;
            return this;
        }
    }
}
