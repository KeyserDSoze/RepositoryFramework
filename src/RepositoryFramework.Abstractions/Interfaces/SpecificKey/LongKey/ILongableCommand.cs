namespace RepositoryFramework
{
    public interface ILongableCommand<T> : ICommand<T, long>, ICommandPattern
    {
    }
}