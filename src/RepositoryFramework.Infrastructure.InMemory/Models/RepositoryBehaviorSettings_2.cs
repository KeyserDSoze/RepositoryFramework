namespace RepositoryFramework.InMemory
{
    /// <summary>
    /// You may set the milliseconds (in range) for each request to simulate a real external database or storage.
    /// You may set a list of exceptions with a random percentage of throwing.
    /// </summary>
    /// <typeparam name="T">Model used for your repository.</typeparam>
    /// <typeparam name="TKey">Key to manage your data from repository.</typeparam>
    public class RepositoryBehaviorSettings<T, TKey> : RepositoryBehaviorSettings<T, TKey, State<T>>
        where TKey : notnull
    {
        internal RepositoryBehaviorSettings() { }
    }
}