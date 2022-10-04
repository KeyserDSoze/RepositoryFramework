using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Azure.Cosmos.Sql
{
    public interface IRepositoryCosmosSqlBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryCosmosSqlBuilder<T, TKey> WithKeyManager<TKeyReader>()
            where TKeyReader : class, ICosmosSqlKeyManager<T, TKey>;
        IRepositoryCosmosSqlBuilder<T, TKey> WithId(Expression<Func<T, TKey>> property);
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}
