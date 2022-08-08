using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed class SerializableQuery
    {
        [JsonPropertyName("o")]
        public List<QueryOperationAsString>? Operations { get; init; }
        public Query Deserialize<T>()
        {
            Query query = new();
            if (Operations != null)
                foreach (var operation in Operations)
                    query.Operations.Add(new(operation.Operation, operation.Expression!.DeserializeAsDynamic<T>(), operation.Value));
            return query;
        }
    }
    public sealed class Query
    {
        public List<QueryOperation> Operations { get; } = new();
        public string Serialize()
        {
            var serialized = new SerializableQuery { Operations = new() };
            foreach (var operation in Operations)
                serialized.Operations.Add(new QueryOperationAsString(operation.Operation,
                    operation.Expression?.Serialize(), operation.Value));
            return serialized.ToJson();
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