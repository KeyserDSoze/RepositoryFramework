namespace RepositoryFramework
{
    public interface IStringableCommand<T> : ICommand<T,string>, ICommandPattern
    {
    }
}