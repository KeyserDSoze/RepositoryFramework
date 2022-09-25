namespace RepositoryFramework
{
    public sealed class State<T> : IState<T>
    {
        public bool IsOk { get; }
        public T? Value { get; }
        public int? Code { get; }
        public string? Message { get; }
        public State() { }
        public State(bool isOk, T? value = default, int? code = default, string? message = default)
        {
            IsOk = isOk;
            Value = value;
            Code = code;
            Message = message;
        }
        public static implicit operator bool(State<T> state)
            => state.IsOk;
        public static implicit operator State<T>(bool state)
            => new(state);
    }
}
