using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    public interface IRepositoryDataverseBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryDataverseBuilder<T, TKey> WithKeyManager<TKeyReader>()
            where TKeyReader : class, IDataverseKeyManager<T, TKey>;
        IRepositoryDataverseBuilder<T, TKey> WithId(Expression<Func<T, TKey>> property);
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}
