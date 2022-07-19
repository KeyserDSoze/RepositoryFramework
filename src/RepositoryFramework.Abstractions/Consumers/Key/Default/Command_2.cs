namespace RepositoryFramework
{
    internal class Command<T, TKey> : Command<T, TKey, State<T>>, ICommand<T, TKey>
        where TKey : notnull
    {
        public Command(ICommandPattern<T, TKey> command) : base(command)
        {
        }
    }
}