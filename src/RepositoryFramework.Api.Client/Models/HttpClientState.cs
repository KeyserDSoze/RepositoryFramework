namespace RepositoryFramework.Api.Client
{
    internal sealed record HttpClientState<T>(bool IsOk, T? Value = default, int? Code = default, string? Message = default) : IState<T>;
    internal sealed record HttpClientEntity<T, TKey>(T Value, TKey Key) : IEntity<T, TKey> where TKey : notnull;
}
