namespace RepositoryFramework.Client
{
    public interface IIntableCommandClient<T> : IIntableCommand<T>, ICommand<T, int>, ICommandPattern
    {
    }
}