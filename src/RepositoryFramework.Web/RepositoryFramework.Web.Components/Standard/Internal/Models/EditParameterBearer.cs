namespace RepositoryFramework.Web.Components.Standard
{
    public sealed class EditParameterBearer
    {
        public required object? BaseEntity { get; set; }
        public Func<object?, Task<object?>>? EntityRetrieverByKey { get; set; }
        public required Dictionary<string, object?> RestorableValues { get; init; }
    }
}
