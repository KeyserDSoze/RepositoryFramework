using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.EntityFramework
{
    internal sealed class RepositoryEntityFrameworkBuilder<T, TKey, TEntityModel> : IRepositoryEntityFrameworkBuilder<T, TKey, TEntityModel>
        where TKey : notnull
        where TEntityModel : class
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
