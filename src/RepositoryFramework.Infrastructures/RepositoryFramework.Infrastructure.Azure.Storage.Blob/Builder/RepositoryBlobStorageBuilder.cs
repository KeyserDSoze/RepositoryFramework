using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    internal sealed class RepositoryBlobStorageBuilder<T, TKey> : IRepositoryBlobStorageBuilder<T, TKey>
        where TKey : notnull
    {
        public IRepositoryBuilder<T, TKey> Builder { get; }
        public RepositoryBlobStorageBuilder(IRepositoryBuilder<T, TKey> builder)
            => Builder = builder;
        public IServiceCollection Services => Builder.Services;
        public PatternType Type => Builder.Type;
        public ServiceLifetime ServiceLifetime => Builder.ServiceLifetime;
        public IQueryTranslationBuilder<T, TKey, TTranslated> Translate<TTranslated>()
            => Builder.Translate<TTranslated>();
        public IRepositoryBlobStorageBuilder<T, TKey> WithIndexing<TProperty>(
            Expression<Func<T, TProperty>> property)
        {
            var name = property.Body.ToString().Split('.').Last();
            var compiledProperty = property.Compile();
            BlobStorageSettings<T, TKey>.Instance.Paths.Add(new BlobStoragePathComposer<T>(x => compiledProperty.Invoke(x)?.ToString(), name));
            Builder.Services.AddSingleton(BlobStorageSettings<T, TKey>.Instance);
            return this;
        }
    }
}
