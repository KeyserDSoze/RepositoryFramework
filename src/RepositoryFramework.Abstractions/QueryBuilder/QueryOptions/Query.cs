using System.Linq.Expressions;
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
        //public IAsyncEnumerable<T> Filter<T>(IAsyncEnumerable<T> enumerable)
        //{
        //    foreach (var operation in Operations)
        //    {
        //        enumerable = operation.Operation switch
        //        {
        //            QueryOperations.Where => enumerable.Where(operation.Expression!.AsExpression<T, bool>()),
        //            QueryOperations.Top => enumerable.Take(operation.Value!.Value),
        //            QueryOperations.Skip => enumerable.Skip(operation.Value!.Value),
        //            QueryOperations.OrderBy => enumerable.OrderBy(operation.Expression!.AsExpression<T, object>()),
        //            QueryOperations.OrderByDescending => enumerable.OrderByDescending(operation.Expression!.AsExpression<T, object>()),
        //            QueryOperations.ThenBy => (enumerable as IOrderedAsyncEnumerable<T>)!.ThenBy(operation.Expression!.AsExpression<T, object>()),
        //            QueryOperations.ThenByDescending => (enumerable as IOrderedAsyncEnumerable<T>)!.ThenByDescending(operation.Expression!.AsExpression<T, object>()),
        //            _ => enumerable,
        //        };
        //    }
        //    return enumerable;
        //}
        public TQueryable Filter<TQueryable, T>(TQueryable queryable)
            where TQueryable : IEnumerable<T> 
            => (TQueryable)Filter(queryable.AsQueryable());
        public IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(IEnumerable<T> enumerable)
            => Filter(enumerable).ToAsyncEnumerable();
        public IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(IQueryable<T> queryable)
            => Filter(queryable).ToAsyncEnumerable();
        public IQueryable<T> Filter<T>(IQueryable<T> queryable)
        {
            foreach (var operation in Operations)
            {
                queryable = operation.Operation switch
                {
                    QueryOperations.Where => queryable.Where(operation.Expression!.AsExpression<T, bool>()).AsQueryable(),
                    QueryOperations.Top => queryable.Take(operation.Value!.Value).AsQueryable(),
                    QueryOperations.Skip => queryable.Skip(operation.Value!.Value).AsQueryable(),
                    QueryOperations.OrderBy => queryable.OrderBy(operation.Expression!.AsExpression<T, object>()).AsQueryable(),
                    QueryOperations.OrderByDescending => queryable.OrderByDescending(operation.Expression!.AsExpression<T, object>()).AsQueryable(),
                    QueryOperations.ThenBy => (queryable as IOrderedQueryable<T>)!.ThenBy(operation.Expression!.AsExpression<T, object>()).AsQueryable(),
                    QueryOperations.ThenByDescending => (queryable as IOrderedQueryable<T>)!.ThenByDescending(operation.Expression!.AsExpression<T, object>()).AsQueryable(),
                    _ => queryable,
                };
            }
            return queryable;
        }
    }
}