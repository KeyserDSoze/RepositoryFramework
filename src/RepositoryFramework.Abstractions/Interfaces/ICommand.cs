namespace RepositoryFramework
{
    public interface ICommand<T, TKey> : ICommandPattern<T, TKey>, ICommandPattern
        where TKey : notnull
    {

    }
}
