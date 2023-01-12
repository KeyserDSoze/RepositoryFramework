using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoryFramework.InMemory
{
    public interface IRepositoryInMemoryBuilder<T, TKey>
        where TKey : notnull
    {
        IServiceCollection Services { get; }
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
