namespace RepositoryFramework
{
    public sealed class State<T> : IState<T>
    {
        public bool IsOk { get; init; }
        public T? Value { get; init; }
        public State() { }
        public State(bool isOk, T? value = default)
        {
            IsOk = isOk;
            Value = value;
        }
        public static implicit operator bool(State<T> state)
            => state.IsOk;
        public static implicit operator State<T>(bool state)
            => new(state);
    }
}
