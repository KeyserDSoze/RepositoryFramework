namespace RepositoryFramework
{
    public interface IState
    {
        public static IState<T> Default<T>(bool isOk, T? value = default)
            => new State<T>(isOk, value);
    }
}
