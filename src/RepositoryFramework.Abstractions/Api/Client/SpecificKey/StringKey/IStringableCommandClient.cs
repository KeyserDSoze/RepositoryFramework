namespace RepositoryFramework.Client
{
    public interface IStringableCommandClient<T> : IStringableCommand<T>, ICommand<T, string>, ICommandPattern
    {
    }
}