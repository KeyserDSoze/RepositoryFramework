using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryBuilder<T, TKey> : IRepositoryBuilder<T, TKey>
        where TKey : notnull
    {
        IRepositoryInMemoryBuilder<TNext, TNextKey> AddRepositoryInMemoryStorage<TNext, TNextKey>(
            Action<RepositoryBehaviorSettings<TNext, TNextKey>>? settings = default)
            where TNextKey : notnull;
        IRepositoryInMemoryBuilder<TNext> AddRepositoryInMemoryStorage<TNext>(
            Action<RepositoryBehaviorSettings<TNext>>? settings = default);
        IRepositoryInMemoryBuilder<T, TKey> PopulateWithJsonData(
            Expression<Func<T, TKey>> navigationKey,
            string json);
        IRepositoryInMemoryBuilder<T, TKey> PopulateWithDataInjection(
            Expression<Func<T, TKey>> navigationKey,
            IEnumerable<T> elements);
        IRepositoryInMemoryCreatorBuilder<T, TKey> PopulateWithRandomData(
            Expression<Func<T, TKey>> navigationKey,
            int numberOfElements = 100,
            int numberOfElementsWhenEnumerableIsFound = 10);
    }
}