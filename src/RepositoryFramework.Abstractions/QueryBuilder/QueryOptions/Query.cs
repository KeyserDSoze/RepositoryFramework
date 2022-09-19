using System.Linq.Expressions;
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
            {
                string? value = null;
                if (operation is LambdaQueryOperation lambda)
                    value = lambda.Expression?.Serialize();
                else if (operation is ValueQueryOperation valueQueryOperation)
                    value = valueQueryOperation.Value.ToString();

                serialized.Operations.Add(new QueryOperationAsString(operation.Operation, value));
            }
            return serialized;
        }
        public SerializableQuery Serialize()
            => ToSerializableQuery();
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
            Operations.Add(new LambdaQueryOperation(QueryOperations.Where, expression));
            return this;
        }
        internal Query Take(int top)
        {
            Operations.Add(new ValueQueryOperation(QueryOperations.Top, top));
            return this;
        }
        internal Query Skip(int skip)
        {
            Operations.Add(new ValueQueryOperation(QueryOperations.Skip, skip));
            return this;
        }
        internal Query OrderBy(LambdaExpression expression)
        {
            Operations.Add(new LambdaQueryOperation(QueryOperations.OrderBy, expression));
            return this;
        }
        internal Query OrderByDescending(LambdaExpression expression)
        {
            Operations.Add(new LambdaQueryOperation(QueryOperations.OrderByDescending, expression));
            return this;
        }
        internal Query ThenBy(LambdaExpression expression)
        {
            Operations.Add(new LambdaQueryOperation(QueryOperations.ThenBy, expression));
            return this;
        }
        internal Query ThenByDescending(LambdaExpression expression)
        {
            Operations.Add(new LambdaQueryOperation(QueryOperations.ThenByDescending, expression));
            return this;
        }
        internal Query GroupBy(LambdaExpression expression)
        {
            Operations.Add(new LambdaQueryOperation(QueryOperations.GroupBy, expression));
            return this;
        }
        internal Query Select(LambdaExpression expression)
        {
            Operations.Add(new LambdaQueryOperation(QueryOperations.Select, expression));
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
                if (operation is LambdaQueryOperation lambda)
                {
                    queryable = lambda.Operation switch
                    {
                        QueryOperations.Where => queryable.Where(lambda.Expression!.AsExpression<T, bool>()).AsQueryable(),
                        QueryOperations.OrderBy => queryable.OrderBy(lambda.Expression!),
                        QueryOperations.OrderByDescending => queryable.OrderByDescending(lambda.Expression!),
                        QueryOperations.ThenBy => (queryable as IOrderedQueryable<T>)!.ThenBy(lambda.Expression!),
                        QueryOperations.ThenByDescending => (queryable as IOrderedQueryable<T>)!.ThenByDescending(lambda.Expression!),
                        _ => queryable,
                    };
                }
                else if (operation is ValueQueryOperation value)
                {
                    queryable = value.Operation switch
                    {
                        QueryOperations.Top => queryable.Take(value.Value != null ? (int)value.Value : 0).AsQueryable(),
                        QueryOperations.Skip => queryable.Skip(value.Value != null ? (int)value.Value : 0).AsQueryable(),
                        _ => queryable,
                    };
                }
            }
            return queryable;
        }
        public IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(IEnumerable<T> enumerable)
        => Filter(enumerable).ToAsyncEnumerable();
        public IAsyncEnumerable<T> FilterAsAsyncEnumerable<T>(IQueryable<T> queryable)
            => Filter(queryable).ToAsyncEnumerable();
        public IQueryable<dynamic> FilterAsSelect<T>(IEnumerable<T> enumerable)
        {
            IQueryable<dynamic>? queryable = null;
            foreach (var item in Operations.Where(x => x.Operation == QueryOperations.Select))
                queryable = enumerable.AsQueryable().Select((item as LambdaQueryOperation)!.Expression!);
            return queryable ?? enumerable.Select(x => (dynamic)x!).AsQueryable();
        }
        public IQueryable<dynamic> FilterAsSelect<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => FilterAsSelect(dictionary.Select(x => x.Value));

        public IQueryable<dynamic> FilterAsSelect<T>(IQueryable<T> queryable)
            => FilterAsSelect(queryable.AsEnumerable());
        public LambdaExpression? FirstSelect => (Operations.FirstOrDefault(x => x.Operation == QueryOperations.Select) as LambdaQueryOperation)?.Expression;
    }
}
