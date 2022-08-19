using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace RepositoryFramework
{
    public sealed class SerializableQuery
    {
        [JsonPropertyName("o")]
        public List<QueryOperationAsString> Operations { get; init; } = new();
        public Query Deserialize<T>()
        {
            Query query = new();
            if (Operations != null)
                foreach (var operation in Operations)
                {
                    if (operation.Operation == QueryOperations.Top || operation.Operation == QueryOperations.Skip)
                        query.Operations.Add(new ValueQueryOperation(operation.Operation, operation.Value != null ? long.Parse(operation.Value) : null));
                    else
                        query.Operations.Add(new LambdaQueryOperation(operation.Operation, operation.Value?.DeserializeAsDynamic<T>()));
                }
            return query;
        }
        public Query DeserializeAndTranslate<T>()
            => FilterTranslation.Instance.Transform<T>(this);
    }
}