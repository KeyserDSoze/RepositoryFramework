namespace RepositoryFramework
{
    public interface IState<out T> : IState
    {
        bool IsOk { get; }
        T? Value { get; }
        int? Code { get; }
        string? Message { get; }
    }
}
