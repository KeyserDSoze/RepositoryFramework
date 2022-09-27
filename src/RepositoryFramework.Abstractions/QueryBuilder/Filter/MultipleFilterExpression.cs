using System.Linq.Expressions;

namespace RepositoryFramework
{
    internal sealed class MultipleFilterExpression : IFilterExpression
    {
        public IFilterExpression FilterByType<T>()
        {
            var name = typeof(T).FullName!;
            if (Filters.ContainsKey(name))
                return Filters[name];
            return FilterByDefault();
        }
        public IFilterExpression FilterByDefault()
        {
            if (Filters.Count > 0)
                return Filters.First().Value;
            else
                return IFilterExpression.Empty;
        }
        public List<FilteringOperation> Operations { get; } = new();
        public SerializableFilter Serialize()
            => FilterByDefault().Serialize();
        public string ToKey()
            => FilterByDefault().ToKey();
        public IFilterExpression Translate<T>()
            => FilterByDefault().Translate<T>();
        public Dictionary<string, FilterExpression> Filters { get; } = new();
        public IQueryable<T> Apply<T>(IEnumerable<T> enumerable)
            => Apply(enumerable.AsQueryable());
        public IQueryable<TValue> Apply<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => Apply(dictionary.Select(x => x.Value).AsQueryable());
        public IQueryable<T> Apply<T>(IQueryable<T> queryable)
            => FilterByType<T>().Apply(queryable);
        public IAsyncEnumerable<T> ApplyAsAsyncEnumerable<T>(IEnumerable<T> enumerable)
            => FilterByType<T>().ApplyAsAsyncEnumerable(enumerable);
        public IAsyncEnumerable<T> ApplyAsAsyncEnumerable<T>(IQueryable<T> queryable)
            => FilterByType<T>().ApplyAsAsyncEnumerable(queryable);
        public IQueryable<dynamic> ApplyAsSelect<T>(IEnumerable<T> enumerable)
            => FilterByType<T>().ApplyAsSelect(enumerable);
        public IQueryable<dynamic> ApplyAsSelect<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => ApplyAsSelect(dictionary.Select(x => x.Value));
        public IQueryable<dynamic> ApplyAsSelect<T>(IQueryable<T> queryable)
            => ApplyAsSelect(queryable.AsEnumerable());
        public IQueryable<IGrouping<dynamic, T>> ApplyAsGroupBy<T>(IEnumerable<T> enumerable)
          => FilterByType<T>().ApplyAsGroupBy(enumerable);
        public IQueryable<IGrouping<dynamic, TValue>> ApplyAsGroupBy<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => ApplyAsGroupBy(dictionary.Select(x => x.Value));
        public IQueryable<IGrouping<dynamic, T>> ApplyAsGroupBy<T>(IQueryable<T> queryable)
            => ApplyAsGroupBy(queryable.AsEnumerable());
        public LambdaExpression? GetFirstSelect<T>()
            => FilterByType<T>().GetFirstSelect<T>();
        public LambdaExpression? DefaultSelect
            => FilterByDefault().DefaultSelect;
        public LambdaExpression? GetFirstGroupBy<T>()
            => FilterByType<T>().GetFirstGroupBy<T>();
        public LambdaExpression? DefaultGroupBy
            => FilterByDefault().DefaultGroupBy;
    }
}
