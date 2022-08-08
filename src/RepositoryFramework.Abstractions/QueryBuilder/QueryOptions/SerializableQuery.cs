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
                    query.Operations.Add(new(operation.Operation, operation.Expression!.DeserializeAsDynamic<T>(), operation.Value));
            return query;
        }
        public Query DeserializeAndTranslate<T>() 
            => FilterTranslation.Instance.Transform<T>(this);
    }
}