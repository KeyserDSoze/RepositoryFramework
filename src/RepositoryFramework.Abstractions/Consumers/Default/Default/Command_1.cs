namespace RepositoryFramework
{
    internal class Command<T> : Command<T, string>, ICommand<T>, ICommandPattern<T>, ICommandPattern
    {
        public Command(ICommandPattern<T> command) : base(command)
        {
        }
    }
}