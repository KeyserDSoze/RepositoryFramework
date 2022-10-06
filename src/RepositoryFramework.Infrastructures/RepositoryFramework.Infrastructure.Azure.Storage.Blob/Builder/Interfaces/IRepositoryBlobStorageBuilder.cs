using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Storage.Blob
{
    public interface IRepositoryBlobStorageBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryBlobStorageBuilder<T, TKey> WithIndexing<TProperty>(
           Expression<Func<T, TProperty>> property);
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}
