namespace RepositoryFramework
{
    /// <summary>
    /// Service registry of all added repository or CQRS services, singletoned and injected in dependency injection.
    /// </summary>
    public class RepositoryFrameworkRegistry
    {
        public static RepositoryFrameworkRegistry Instance { get; } = new();
        public List<RepositoryFrameworkService> Services { get; } = new();
        private RepositoryFrameworkRegistry() { }
    }
}