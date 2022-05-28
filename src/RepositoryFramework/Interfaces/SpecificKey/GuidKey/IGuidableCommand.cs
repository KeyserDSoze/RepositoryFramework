namespace RepositoryFramework
{
    public interface IGuidableCommand<T> : ICommand<T, Guid>, ICommandPattern
    {
    }
}