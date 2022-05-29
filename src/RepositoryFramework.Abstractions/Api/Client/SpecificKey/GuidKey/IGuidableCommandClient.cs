namespace RepositoryFramework.Client
{
    public interface IGuidableCommandClient<T> : IGuidableCommand<T>, ICommand<T, Guid>, ICommandPattern
    {
    }
}