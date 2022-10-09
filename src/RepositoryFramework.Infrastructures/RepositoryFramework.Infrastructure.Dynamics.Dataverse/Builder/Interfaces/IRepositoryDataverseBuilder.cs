using System.Linq.Expressions;

namespace RepositoryFramework.Infrastructure.Dynamics.Dataverse
{
    public interface IRepositoryDataverseBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryDataverseBuilder<T, TKey> WithColumn<TProperty>(Expression<Func<T, TProperty>> property,
             string? customPrefix = null);
        IRepositoryBuilder<T, TKey> Builder { get; }
    }
}
