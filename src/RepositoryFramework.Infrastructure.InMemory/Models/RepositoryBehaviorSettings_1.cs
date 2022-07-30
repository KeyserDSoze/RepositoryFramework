namespace RepositoryFramework.InMemory
{
    /// <summary>
    /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
    /// You may set a list of exceptions with a random percentage of throwing.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    public class RepositoryBehaviorSettings<T> : RepositoryBehaviorSettings<T, string>
    {
        internal RepositoryBehaviorSettings() { }
    }
}