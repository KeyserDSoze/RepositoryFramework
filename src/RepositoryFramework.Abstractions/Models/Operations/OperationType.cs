using System.Linq.Expressions;

namespace RepositoryFramework
{
    public sealed class OperationType<TProperty>
    {
        public static OperationType<TProperty> Count { get; } = new() { Operation = Operations.Count };
        public static OperationType<TProperty> Sum { get; } = new() { Operation = Operations.Sum };
        public static OperationType<TProperty> Max { get; } = new() { Operation = Operations.Max };
        public static OperationType<TProperty> Min { get; } = new() { Operation = Operations.Min };
        public static OperationType<TProperty> Average { get; } = new() { Operation = Operations.Average };
        public Operations Operation { get; init; }
        private Type? type;
        public Type Type => type ??= typeof(TProperty);
        public ValueTask<TProperty?> ExecuteAsync(
            Delegate count,
            Delegate sum,
            Delegate max,
            Delegate min,
            Delegate average)
            => Operation switch
            {
                Operations.Count => count.InvokeAsync<TProperty>(),
                Operations.Sum => sum.InvokeAsync<TProperty>(),
                Operations.Max => max.InvokeAsync<TProperty>(),
                Operations.Min => min.InvokeAsync<TProperty>(),
                Operations.Average => average.InvokeAsync<TProperty>(),
                _ => throw new NotImplementedException($"{Operation} not found")
            };
    }
}