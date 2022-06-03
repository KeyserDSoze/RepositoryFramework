namespace RepositoryFramework
{
    /// <summary>
    /// Service registry of all repository or CQRS services added, singletoned and injected in dependency injection.
    /// </summary>
    public class RepositoryFrameworkRegistry
    {
        public static RepositoryFrameworkRegistry Instance { get; } = new();
        public List<RepositoryFrameworkService> Services { get; } = new();
        private RepositoryFrameworkRegistry() { }
    }
}