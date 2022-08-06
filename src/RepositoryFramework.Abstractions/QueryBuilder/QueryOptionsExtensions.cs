namespace RepositoryFramework
{
    public static partial class QueryOptionsExtensions
    {
        /// <summary>
        /// Help your context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IQueryable<typeparamref name="T"/></returns>
        public static IQueryable<T> Filter<T>(this IEnumerable<T> items, QueryOptions<T>? options)
        {
            IQueryable<T> query = items.AsQueryable();
            if (options != null)
            {
                if (options.Predicate != null)
                    query = query.Where(options.Predicate).AsQueryable();
                foreach (var order in options.Orders)
                    if (!order.ThenBy)
                    {
                        if (order.IsAscending)
                            query = query.OrderBy(order.Order).AsQueryable();
                        else
                            query = query.OrderByDescending(order.Order).AsQueryable();
                    }
                    else
                    {
                        if (query is IOrderedQueryable<T> ordered)
                            if (order.IsAscending)
                                query = ordered.ThenBy(order.Order).AsQueryable();
                            else
                                query = ordered.ThenByDescending(order.Order).AsQueryable();
                    }
                if (options.Skip != null)
                    query = query.Skip(options.Skip.Value);
                if (options.Top != null)
                    query = query.Take(options.Top.Value);
            }
            return query;
        }
        /// <summary>
        /// Help your Dictionary/KeyValuePair context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePair">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IQueryable<typeparamref name="TValue"/></returns>
        public static IQueryable<TValue> Filter<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePair,
           QueryOptions<TValue>? options)
           => keyValuePair.Select(x => x.Value).Filter(options);
        /// <summary>
        /// Help your context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="T"/></returns>
        public static IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(this IEnumerable<T> items, QueryOptions<T>? options)
            => items.Filter(options).ToAsyncEnumerable();
        /// <summary>
        /// Help your Dictionary/KeyValuePair context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePair">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="TValue"/></returns>
        public static IAsyncEnumerable<TValue> FilterAsAsyncEnumerable<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePair,
           QueryOptions<TValue>? options)
           => keyValuePair.Select(x => x.Value).FilterAsAsyncEnumerable(options);
        /// <summary>
        /// Help your async context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="T"/></returns>
        public static IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(this IAsyncEnumerable<T> items, QueryOptions<T>? options)
        {
            if (options != null)
            {
                if (options.Predicate != null)
                    items = items.Where(options.Predicate.Compile());
                foreach (var order in options.Orders)
                    if (!order.ThenBy)
                    {
                        if (order.IsAscending)
                            items = items.OrderBy(order.Order.Compile());
                        else
                            items = items.OrderByDescending(order.Order.Compile());
                    }
                    else
                    {
                        if (items is IOrderedAsyncEnumerable<T> ordered)
                            if (order.IsAscending)
                                items = ordered.ThenBy(order.Order.Compile());
                            else
                                items = ordered.ThenByDescending(order.Order.Compile());
                    }
                if (options.Skip != null)
                    items = items.Skip(options.Skip.Value);
                if (options.Top != null)
                    items = items.Take(options.Top.Value);
            }
            return items;
        }
        /// <summary>
        /// Help your Dictionary/KeyValuePair async context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePair">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="TValue"/></returns>
        public static IAsyncEnumerable<TValue> FilterAsAsyncEnumerable<TKey, TValue>(this IAsyncEnumerable<KeyValuePair<TKey, TValue>> keyValuePair,
           QueryOptions<TValue>? options)
           => keyValuePair.Select(x => x.Value).FilterAsAsyncEnumerable(options);
    }
}