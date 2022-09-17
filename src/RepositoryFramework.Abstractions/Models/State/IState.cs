namespace RepositoryFramework
{
    public interface IState
    {
        public static IState<T> Default<T>(bool isOk, T? value = default, int? code = default, string? message = default)
            => new State<T>(isOk, value, code, message);
        public static IState<T> Ok<T>() => Default<T>(true);
        public static IState<T> NotOk<T>() => Default<T>(false);
    }
}
