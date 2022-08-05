namespace RepositoryFramework
{
    public static partial class QueryOptionsExtensions
    {
        /// <summary>
        /// Help your context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TToTranslate"></typeparam>
        /// <param name="items">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IQueryable<typeparamref name="T"/></returns>
        public static IQueryable<T> FilterWithTranslation<T, TToTranslate>(this IEnumerable<T> items, QueryOptions<TToTranslate>? options) 
            => items.Filter(options?.Translate<T>());
        /// <summary>
        /// Help your Dictionary/KeyValuePair context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePair">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IQueryable<typeparamref name="TValue"/></returns>
        public static IQueryable<TValue> FilterWithTranslation<TKey, TValue, TValueToTranslate>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePair,
           QueryOptions<TValueToTranslate>? options) 
            => keyValuePair.Filter(options?.Translate<TValue>());

        /// <summary>
        /// Help your context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TToTranslate"></typeparam>
        /// <param name="items">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="T"/></returns>
        public static IAsyncEnumerable<T> FilterWithTranslationAsAsyncEnumerable<T, TToTranslate>(this IEnumerable<T> items,
            QueryOptions<TToTranslate>? options)
            => items.FilterAsAsyncEnumerable(options?.Translate<T>());
        /// <summary>
        /// Help your Dictionary/KeyValuePair context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TValueToTranslate"></typeparam>
        /// <param name="keyValuePair">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="TValue"/></returns>
        public static IAsyncEnumerable<TValue> FilterWithTranslationAsAsyncEnumerable<TKey, TValue, TValueToTranslate>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePair,
           QueryOptions<TValueToTranslate>? options)
           => keyValuePair.FilterAsAsyncEnumerable(options?.Translate<TValue>());
        /// <summary>
        /// Help your async context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TToTranslate"></typeparam>
        /// <param name="items">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="T"/></returns>
        public static IAsyncEnumerable<T> FilterWithTranslationAsAsyncEnumerable<T, TToTranslate>(this IAsyncEnumerable<T> items,
            QueryOptions<TToTranslate>? options) 
            => items.FilterAsAsyncEnumerable(options?.Translate<T>());
        /// <summary>
        /// Help your Dictionary/KeyValuePair async context to apply filters. Use in options the Translate method to change the name of the properties.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TValueToTranslate"></typeparam>
        /// <param name="keyValuePair">Context.</param>
        /// <param name="options">Query options.</param>
        /// <returns>IAsyncEnumerable<typeparamref name="TValue"/></returns>
        public static IAsyncEnumerable<TValue> FilterWithTranslationAsAsyncEnumerable<TKey, TValue, TValueToTranslate>(this IAsyncEnumerable<KeyValuePair<TKey, TValue>> keyValuePair,
           QueryOptions<TValueToTranslate>? options)
           => keyValuePair.FilterAsAsyncEnumerable(options?.Translate<TValue>());
    }
}