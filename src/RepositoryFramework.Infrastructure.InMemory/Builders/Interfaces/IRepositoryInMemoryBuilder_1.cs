using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryBuilder<T> : IRepositoryInMemoryBuilder<T, string>, IRepositoryBuilder<T>
    {
        new IRepositoryInMemoryBuilder<T> PopulateWithJsonData(
            Expression<Func<T, string>> navigationKey,
            string json);
        new IRepositoryInMemoryBuilder<T> PopulateWithDataInjection(
            Expression<Func<T, string>> navigationKey,
            IEnumerable<T> elements);
        new IRepositoryInMemoryCreatorBuilder<T> PopulateWithRandomData(
            Expression<Func<T, string>> navigationKey,
            int numberOfElements = 100,
            int numberOfElementsWhenEnumerableIsFound = 10);
    }
}