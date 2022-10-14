using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.EntityFramework
{
    internal sealed class RepositoryEntityFrameworkBuilder<T, TKey> : IRepositoryEntityFrameworkBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public RepositoryEntityFrameworkBuilder(IRepositoryBuilder<T, TKey> builder)
            => Builder = builder;
        public IServiceCollection Services => Builder.Services;
        public PatternType Type => Builder.Type;
        public ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
    }
}
