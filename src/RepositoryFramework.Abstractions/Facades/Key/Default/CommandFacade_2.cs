namespace RepositoryFramework
{
    internal class CommandFacade<T, TKey> : CommandFacade<T, TKey, bool>, ICommandFacade<T, TKey>, ICommandPattern<T, TKey>, ICommandPattern
         where TKey : notnull
    {
        public CommandFacade(ICommandPattern<T, TKey> command) : base(command)
        {
        }
    }
}