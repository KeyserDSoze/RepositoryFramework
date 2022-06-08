namespace RepositoryFramework
{
    internal class CommandFacade<T> : CommandFacade<T, string>, ICommandFacade<T>, ICommandPattern<T>, ICommandPattern
    {
        public CommandFacade(ICommandPattern<T> command) : base(command)
        {
        }
    }
}