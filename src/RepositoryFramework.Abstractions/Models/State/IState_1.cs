namespace RepositoryFramework
{
    public interface IState<out T> : IState
    {
        bool IsOk { get; }
        T? Value { get; }
    }
}
