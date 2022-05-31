namespace RepositoryFramework
{
    public class RepositoryFrameworkServices
    {
        public static RepositoryFrameworkServices Instance { get; } = new();
        public List<RepositoryFrameworkService> Services { get; } = new();
        private RepositoryFrameworkServices() { }
    }
}