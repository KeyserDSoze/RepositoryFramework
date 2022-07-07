namespace RepositoryFramework
{
    internal class Command<T, TKey> : Command<T, TKey, bool>, ICommand<T, TKey>
        where TKey : notnull
    {
        public Command(ICommandPattern<T, TKey> command) : base(command)
        {
        }
    }
}