using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace RepositoryFramework.InMemory
{
    internal class RepositoryInMemoryBuilder<T> : RepositoryInMemoryBuilder<T, string>, IRepositoryInMemoryBuilder<T>
    {
        public RepositoryInMemoryBuilder(IServiceCollection services) :
            base(services)
        {
            _numberOfParameters = 1;
        }
        public new IRepositoryInMemoryBuilder<T> PopulateWithDataInjection(Expression<Func<T, string>> navigationKey, IEnumerable<T> elements)
        {
            _ = base.PopulateWithDataInjection(navigationKey, elements);
            return this;
        }

        public new IRepositoryInMemoryBuilder<T> PopulateWithJsonData(Expression<Func<T, string>> navigationKey, string json)
        {
            _ = base.PopulateWithJsonData(navigationKey, json);
            return this;
        }

        public new IRepositoryInMemoryCreatorBuilder<T> PopulateWithRandomData(Expression<Func<T, string>> navigationKey, int numberOfElements = 100, int numberOfElementsWhenEnumerableIsFound = 10)
        {
            _ = base.PopulateWithRandomData(navigationKey, numberOfElements, numberOfElementsWhenEnumerableIsFound);
            return new RepositoryInMemoryCreatorBuilder<T>(this, _internalBehaviorSettings);
        }

        public new IQueryTranslationInMemoryBuilder<T, TTranslated> Translate<TTranslated>()
           => new QueryTranslationInMemoryBuilder<T, TTranslated>(this);

        IQueryTranslationBuilder<T, TTranslated> IRepositoryBuilder<T>.Translate<TTranslated>() 
            => Translate<TTranslated>();
    }
}