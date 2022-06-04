namespace RepositoryFramework
{
    internal class Command<T, TKey> : Command<T, TKey, bool>, ICommand<T, TKey>, ICommandPattern<T, TKey>, ICommandPattern
         where TKey : notnull
    {
        public Command(ICommandPattern<T, TKey> command) : base(command)
        {
        }
    }
}