namespace RepositoryFramework
{
    public static class StateExtensions
    {
        public static IState<T> ToState<T>(this bool value)
            => IState.Default<T>(value);
    }
}
