namespace RepositoryFramework
{
    internal class RepositoryFacade<T, TKey> : RepositoryFacade<T, TKey, bool>, IRepositoryFacade<T, TKey>, IRepositoryPattern<T, TKey>, ICommandPattern<T, TKey>, IQueryPattern<T, TKey>, IRepositoryPattern, ICommandPattern, IQueryPattern
         where TKey : notnull
    {
        public RepositoryFacade(IRepositoryPattern<T, TKey> repository) : base(repository)
        {
        }
    }
}