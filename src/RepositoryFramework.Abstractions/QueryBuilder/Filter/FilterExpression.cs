using System.Linq.Expressions;

namespace RepositoryFramework
{
    public sealed class FilterExpression : IFilterExpression
    {
        public static FilterExpression Empty => new();
        public List<FilteringOperation> Operations { get; } = new();
        private SerializableFilter ToSerializableQuery()
        {
            var serialized = new SerializableFilter { Operations = new() };
            foreach (var operation in Operations)
            {
                string? value = null;
                if (operation is LambdaFilterOperation lambda)
                    value = lambda.Expression?.Serialize();
                else if (operation is ValueFilterOperation valueQueryOperation)
                    value = valueQueryOperation.Value.ToString();

                serialized.Operations.Add(new FilterOperationAsString(operation.Operation, value));
            }
            return serialized;
        }
        public SerializableFilter Serialize()
            => ToSerializableQuery();
        public string ToKey()
            => ToSerializableQuery().ToString()!;
        public IFilterExpression Translate<T>()
        {
            if (FilterTranslation.Instance.HasTranslation<T>())
                return ToSerializableQuery().DeserializeAndTranslate<T>();
            return this;
        }
        internal FilterExpression Where(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.Where, expression));
            return this;
        }
        internal FilterExpression Take(int top)
        {
            Operations.Add(new ValueFilterOperation(FilterOperations.Top, top));
            return this;
        }
        internal FilterExpression Skip(int skip)
        {
            Operations.Add(new ValueFilterOperation(FilterOperations.Skip, skip));
            return this;
        }
        internal FilterExpression OrderBy(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.OrderBy, expression));
            return this;
        }
        internal FilterExpression OrderByDescending(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.OrderByDescending, expression));
            return this;
        }
        internal FilterExpression ThenBy(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.ThenBy, expression));
            return this;
        }
        internal FilterExpression ThenByDescending(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.ThenByDescending, expression));
            return this;
        }
        internal FilterExpression GroupBy(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.GroupBy, expression));
            return this;
        }
        internal FilterExpression Select(LambdaExpression expression)
        {
            if (expression != null)
                Operations.Add(new LambdaFilterOperation(FilterOperations.Select, expression));
            return this;
        }
        public IQueryable<T> Apply<T>(IEnumerable<T> enumerable)
            => Apply(enumerable.AsQueryable());
        public IQueryable<TValue> Apply<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => Apply(dictionary.Select(x => x.Value).AsQueryable());
        public IQueryable<T> Apply<T>(IQueryable<T> queryable)
        {
            foreach (var operation in Operations)
            {
                if (operation is LambdaFilterOperation lambda && lambda.Expression != null)
                {
                    queryable = lambda.Operation switch
                    {
                        FilterOperations.Where => queryable.Where(lambda.Expression.AsExpression<T, bool>()).AsQueryable(),
                        FilterOperations.OrderBy => queryable.OrderBy(lambda.Expression),
                        FilterOperations.OrderByDescending => queryable.OrderByDescending(lambda.Expression),
                        FilterOperations.ThenBy => (queryable as IOrderedQueryable<T>)!.ThenBy(lambda.Expression),
                        FilterOperations.ThenByDescending => (queryable as IOrderedQueryable<T>)!.ThenByDescending(lambda.Expression),
                        _ => queryable,
                    };
                }
                else if (operation is ValueFilterOperation value)
                {
                    queryable = value.Operation switch
                    {
                        FilterOperations.Top => queryable.Take(value.Value != null ? (int)value.Value : 0).AsQueryable(),
                        FilterOperations.Skip => queryable.Skip(value.Value != null ? (int)value.Value : 0).AsQueryable(),
                        _ => queryable,
                    };
                }
            }
            return queryable;
        }
        public IAsyncEnumerable<T> ApplyAsAsyncEnumerable<T>(IEnumerable<T> enumerable)
            => Apply(enumerable).ToAsyncEnumerable();
        public IAsyncEnumerable<T> ApplyAsAsyncEnumerable<T>(IQueryable<T> queryable)
            => Apply(queryable).ToAsyncEnumerable();
        public IQueryable<dynamic> ApplyAsSelect<T>(IEnumerable<T> enumerable)
        {
            IQueryable<dynamic>? queryable = null;
            foreach (var lambda in Operations.Where(x => x.Operation == FilterOperations.Select).Select(x => x as LambdaFilterOperation))
                if (lambda?.Expression != null)
                    queryable = enumerable.AsQueryable().Select(lambda.Expression);
            return queryable ?? enumerable.Select(x => (dynamic)x!).AsQueryable();
        }
        public IQueryable<dynamic> ApplyAsSelect<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            => ApplyAsSelect(dictionary.Select(x => x.Value));

        public IQueryable<dynamic> ApplyAsSelect<T>(IQueryable<T> queryable)
            => ApplyAsSelect(queryable.AsEnumerable());
        public LambdaExpression? GetFirstSelect<T>()
            => DefaultSelect;
        public LambdaExpression? DefaultSelect
            => (Operations.FirstOrDefault(x => x.Operation == FilterOperations.Select) as LambdaFilterOperation)?.Expression;
    }
}
