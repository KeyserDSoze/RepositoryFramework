using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryBuilder<T, TKey, TState> : IRepositoryBuilder<T, TKey, TState>
        where TKey : notnull
        where TState : class, IState<T>, new()
    {
        IRepositoryInMemoryBuilder<TNext, TNextKey, TNextState> AddRepositoryInMemoryStorage<TNext, TNextKey, TNextState>(
            Action<RepositoryBehaviorSettings<TNext, TNextKey, TNextState>>? settings = default)
            where TNextKey : notnull
            where TNextState : class, IState<TNext>, new();
        IRepositoryInMemoryBuilder<TNext, TNextKey> AddRepositoryInMemoryStorage<TNext, TNextKey>(
            Action<RepositoryBehaviorSettings<TNext, TNextKey>>? settings = default)
            where TNextKey : notnull;
        IRepositoryInMemoryBuilder<TNext> AddRepositoryInMemoryStorage<TNext>(
            Action<RepositoryBehaviorSettings<TNext>>? settings = default);
        IRepositoryInMemoryBuilder<T, TKey, TState> PopulateWithJsonData(
            Expression<Func<T, TKey>> navigationKey,
            string json);
        IRepositoryInMemoryBuilder<T, TKey, TState> PopulateWithDataInjection(
            Expression<Func<T, TKey>> navigationKey,
            IEnumerable<T> elements);
        IRepositoryInMemoryCreatorBuilder<T, TKey, TState> PopulateWithRandomData(
            Expression<Func<T, TKey>> navigationKey,
            int numberOfElements = 100,
            int numberOfElementsWhenEnumerableIsFound = 10);
    }
}