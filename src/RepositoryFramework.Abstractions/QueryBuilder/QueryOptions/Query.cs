﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace RepositoryFramework
{
    public sealed class Query
    {
        public static Query Empty => new();
        public List<QueryOperation> Operations { get; } = new();
        public SerializableQuery ToSerializableQuery()
        {
            var serialized = new SerializableQuery { Operations = new() };
            foreach (var operation in Operations)
                serialized.Operations.Add(new QueryOperationAsString(operation.Operation,
                    operation.Expression?.Serialize(), operation.Value));
            return serialized;
        }
        public string Serialize()
            => ToSerializableQuery().ToJson();
        public string ToKey()
            => ToSerializableQuery().ToString()!;
        public Query Translate<T>()
        {
            if (FilterTranslation.Instance.HasTranslation<T>())
                return ToSerializableQuery().DeserializeAndTranslate<T>();
            return this;
        }
        internal Query Where(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.Where, expression));
            return this;
        }
        internal Query Take(int top)
        {
            Operations.Add(new QueryOperation(QueryOperations.Top, null, top));
            return this;
        }
        internal Query Skip(int skip)
        {
            Operations.Add(new QueryOperation(QueryOperations.Skip, null, skip));
            return this;
        }
        internal Query OrderBy(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.OrderBy, expression));
            return this;
        }
        internal Query OrderByDescending(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.OrderByDescending, expression));
            return this;
        }
        internal Query ThenBy(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.ThenBy, expression));
            return this;
        }
        internal Query ThenByDescending(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.ThenByDescending, expression));
            return this;
        }
        internal Query GroupBy(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.GroupBy, expression));
            return this;
        }
        internal Query Select(LambdaExpression expression)
        {
            Operations.Add(new QueryOperation(QueryOperations.Select, expression));
            return this;
        }
        public IQueryable<T> Filter<T>(IEnumerable<T> enumerable)
            => Filter(enumerable.AsQueryable());
        public IQueryable<TValue> Filter<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => Filter(dictionary.Select(x => x.Value).AsQueryable());
        public IQueryable<T> Filter<T>(IQueryable<T> queryable)
        {
            foreach (var operation in Operations)
            {
                queryable = operation.Operation switch
                {
                    QueryOperations.Where => queryable.Where(operation.Expression!.AsExpression<T, bool>()).AsQueryable(),
                    QueryOperations.Top => queryable.Take(operation.Value!.Value).AsQueryable(),
                    QueryOperations.Skip => queryable.Skip(operation.Value!.Value).AsQueryable(),
                    QueryOperations.OrderBy => queryable.OrderBy(operation.Expression!),
                    QueryOperations.OrderByDescending => queryable.OrderByDescending(operation.Expression!),
                    QueryOperations.ThenBy => (queryable as IOrderedQueryable<T>)!.ThenBy(operation.Expression!),
                    QueryOperations.ThenByDescending => (queryable as IOrderedQueryable<T>)!.ThenByDescending(operation.Expression!),
                    _ => queryable,
                };
            }
            return queryable;
        }
        public IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(IEnumerable<T> enumerable)
        => Filter(enumerable).ToAsyncEnumerable();
        public IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(IQueryable<T> queryable)
            => Filter(queryable).ToAsyncEnumerable();
        public IQueryable<object> FilterAsSelect<T>(IEnumerable<T> enumerable)
        {
            IQueryable<object>? queryable = null;
            foreach (var item in Operations.Where(x => x.Operation == QueryOperations.Select))
                queryable = enumerable.AsQueryable().Select(item.Expression!);
            return queryable ?? enumerable.Select(x => (object)x!).AsQueryable();
        }
        public IQueryable<object> FilterAsSelect<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => FilterAsSelect(dictionary.Select(x => x.Value));

        public IQueryable<object> FilterAsSelect<T>(IQueryable<T> queryable)
            => FilterAsSelect(queryable.AsEnumerable());
    }
}