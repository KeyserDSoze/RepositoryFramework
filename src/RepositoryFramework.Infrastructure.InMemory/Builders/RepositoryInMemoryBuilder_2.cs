using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryBuilder<T, TKey> : RepositoryInMemoryBuilder<T, TKey, State<T>>, IRepositoryInMemoryBuilder<T, TKey>
        where TKey : notnull
    {
        public RepositoryInMemoryBuilder(IServiceCollection services) : base(services)
        {
            _numberOfParameters = 2;
        }

        public new IRepositoryInMemoryBuilder<T, TKey> PopulateWithDataInjection(Expression<Func<T, TKey>> navigationKey, IEnumerable<T> elements)
        {
            _ = base.PopulateWithDataInjection(navigationKey, elements);
            return this;
        }

        public new IRepositoryInMemoryBuilder<T, TKey> PopulateWithJsonData(Expression<Func<T, TKey>> navigationKey, string json)
        {
            _ = base.PopulateWithJsonData(navigationKey, json);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T, TKey> PopulateWithRandomData(Expression<Func<T, TKey>> navigationKey, int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
        {
            _ = base.PopulateWithRandomData(navigationKey, numberOfElements, numberOfElementsWhenEnumerableIsFound);
            return new RepositoryInMemoryCreatorBuilder<T, TKey>(this, _internalBehaviorSettings);
        }
    }
}