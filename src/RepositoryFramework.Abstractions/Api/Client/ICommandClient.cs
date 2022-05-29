namespace RepositoryFramework.Client
{
    public interface ICommandClient<T, TKey> : ICommand<T, TKey>, ICommandPattern
        where TKey : notnull
    {
    }
}