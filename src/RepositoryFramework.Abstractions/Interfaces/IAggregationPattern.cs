namespace RepositoryFramework
{
    /// <summary>
    /// Interface for your CQRS pattern, with Count, Sum, Max, Min, Average, GroupBy methods.
    /// This is the interface that you need to extend if you want to create your query pattern.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to retrieve your data from repository.</typeparam>
    public interface IAggregationPattern<T, TKey>
        where TKey : notnull
    {
        ValueTask<TProperty> CountAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default);
        ValueTask<TProperty> SumAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default);
        ValueTask<TProperty> MaxAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default);
        ValueTask<TProperty> MinAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default);
        ValueTask<TProperty> AverageAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default);
        IAsyncEnumerable<IGrouping<TProperty, IEntity<T, TKey>>> GroupByAsync<TProperty>(IFilterExpression filter, CancellationToken cancellationToken = default);
    }
}
