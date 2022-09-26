namespace RepositoryFramework
{
    internal sealed class Aggregation<T, TKey> : IAggregation<T, TKey>
        where TKey : notnull
    {
        private readonly IAggregationPattern<T, TKey> _aggregation;
        private readonly IRepositoryBusinessManager<T, TKey>? _businessManager;

        public Aggregation(IAggregationPattern<T, TKey> aggregation,
            IRepositoryBusinessManager<T, TKey>? businessManager = null)
        {
            _aggregation = aggregation;
            _businessManager = businessManager;
        }

        public ValueTask<TProperty> AverageAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _aggregation.AverageAsync<TProperty>(filter, cancellationToken);

        public ValueTask<TProperty> CountAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _aggregation.CountAsync<TProperty>(filter, cancellationToken);

        public IAsyncEnumerable<IAsyncGrouping<TProperty, IEntity<T, TKey>>> GroupByAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _aggregation.GroupByAsync<TProperty>(filter, cancellationToken);

        public ValueTask<TProperty> MaxAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _aggregation.MaxAsync<TProperty>(filter, cancellationToken);

        public ValueTask<TProperty> MinAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _aggregation.MinAsync<TProperty>(filter, cancellationToken);

        public ValueTask<TProperty> SumAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default)
            => _aggregation.SumAsync<TProperty>(filter, cancellationToken);
    }
}
