namespace RepositoryFramework
{
    internal sealed class RepositoryFrameworkAfterServiceBuildEvents
    {
        public static RepositoryFrameworkAfterServiceBuildEvents Instance { get; } = new();
        private RepositoryFrameworkAfterServiceBuildEvents() { }
        public List<Func<IServiceProvider, Task>> Events { get; set; } = new();
        public void AddEvent(Func<IServiceProvider, Task> task)
            => Events.Add(task);
    }
}
