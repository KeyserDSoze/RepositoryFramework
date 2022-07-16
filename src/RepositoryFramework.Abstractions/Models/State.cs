namespace RepositoryFramework
{
    public sealed record State(bool IsOk) : IState
    {
        public static implicit operator bool(State state) 
            => state.IsOk;
        public static implicit operator State(bool state) 
            => new(state);
    }
}
