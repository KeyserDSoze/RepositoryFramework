namespace RepositoryFramework.Client
{
    public interface ILongableCommandClient<T> : ILongableCommand<T>, ICommand<T, long>, ICommandPattern
    {
    }
}