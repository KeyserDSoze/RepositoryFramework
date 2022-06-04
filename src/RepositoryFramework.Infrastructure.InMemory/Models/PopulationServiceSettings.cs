namespace RepositoryFramework.InMemory
{
    public class PopulationServiceSettings<T, TKey>
        where TKey : notnull
    {
        public Action<TKey, T>? AddElementToMemory { get; init; }
        public CreationSettings? BehaviorSettings { get; init; }
        public string? KeyName { get; init; }
        public int NumberOfElements { get; init; }
        public int NumberOfElementsWhenEnumerableIsFound { get; init; }
    }
}