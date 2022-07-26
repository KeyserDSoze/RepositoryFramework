namespace RepositoryFramework
{
    public static class QueryOptionsExtensions
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
            if (options != null)
            {
                if (options.Predicate != null)
                    items = items.Where(options.Predicate.Compile());
                if (options.Order != null)
                    if (options.IsAscending)
                        items = items.OrderBy(options.Order.Compile());
                    else
                        items = items.OrderByDescending(options.Order.Compile());
                if (options.Skip != null)
                    items = items.Skip(options.Skip.Value);
                if (options.Top != null)
                    items = items.Take(options.Top.Value);
            }
            return items.AsQueryable();
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
        {
            if (options != null)
            {
                if (options.Predicate != null)
                    items = items.Where(options.Predicate.Compile());
                if (options.Order != null)
                    if (options.IsAscending)
                        items = items.OrderBy(options.Order.Compile());
                    else
                        items = items.OrderByDescending(options.Order.Compile());
                if (options.Skip != null)
                    items = items.Skip(options.Skip.Value);
                if (options.Top != null)
                    items = items.Take(options.Top.Value);
            }
            return items.ToAsyncEnumerable();
        }
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
                if (options.Order != null)
                    if (options.IsAscending)
                        items = items.OrderBy(options.Order.Compile());
                    else
                        items = items.OrderByDescending(options.Order.Compile());
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