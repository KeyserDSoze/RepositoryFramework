namespace RepositoryFramework
{
    internal class Command<T> : Command<T, string>, ICommand<T>
    {
        public Command(ICommandPattern<T> command) : base(command)
        {
        }
    }
}