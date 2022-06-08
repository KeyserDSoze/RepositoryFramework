namespace RepositoryFramework
{
    internal class RepositoryFacade<T> : RepositoryFacade<T, string>, IRepositoryFacade<T>, IRepositoryPattern<T>, ICommandPattern<T>, IQueryPattern<T>, IRepositoryPattern, ICommandPattern, IQueryPattern
    {
        public RepositoryFacade(IRepositoryPattern<T> repository) : base(repository)
        {
        }
    }
}