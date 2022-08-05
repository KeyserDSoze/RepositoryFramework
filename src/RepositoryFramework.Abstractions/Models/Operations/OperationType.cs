using System.Linq.Expressions;

namespace RepositoryFramework
{
    public sealed class OperationType<TProperty>
    {
        public static OperationType<TProperty> Count { get; } = new() { Type = Operations.Count };
        public static OperationType<TProperty> Sum { get; } = new() { Type = Operations.Sum };
        public static OperationType<TProperty> Max { get; } = new() { Type = Operations.Max };
        public static OperationType<TProperty> Min { get; } = new() { Type = Operations.Min };
        public static OperationType<TProperty> Average { get; } = new() { Type = Operations.Average };
        public Operations Type { get; init; }
        public ValueTask<TProperty?> ExecuteAsync(
            Delegate count,
            Delegate sum,
            Delegate max,
            Delegate min,
            Delegate average)
            => Type switch
            {
                Operations.Count => count.InvokeAsync<TProperty>(),
                Operations.Sum => sum.InvokeAsync<TProperty>(),
                Operations.Max => max.InvokeAsync<TProperty>(),
                Operations.Min => min.InvokeAsync<TProperty>(),
                Operations.Average => average.InvokeAsync<TProperty>(),
                _ => throw new NotImplementedException($"{Type} not found")
            };
    }
}