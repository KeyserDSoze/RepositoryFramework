namespace RepositoryFramework
{
    /// <summary>
    /// Interface for return state.
    /// </summary>
    public interface IState<T>
    {
        bool IsOk { get; init; }
        T? Value { get; init; }
    }
}
