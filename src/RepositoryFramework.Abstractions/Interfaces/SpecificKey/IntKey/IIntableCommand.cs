namespace RepositoryFramework
{
    public interface IIntableCommand<T> : ICommand<T, int>, ICommandPattern
    {
    }
}