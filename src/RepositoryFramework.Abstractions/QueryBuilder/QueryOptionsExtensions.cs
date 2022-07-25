﻿namespace RepositoryFramework
{
    public static class QueryOptionsExtensions
    {
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
        public static IQueryable<TValue> Filter<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> hashMap,
            QueryOptions<TValue>? options) 
            => hashMap.Select(x => x.Value).Filter(options);
    }
}