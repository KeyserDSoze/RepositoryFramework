namespace RepositoryFramework.Api.Client
{
    internal sealed record HttpClientState<T>(bool IsOk, T? Value = default, int? Code = default, string? Message = default) : IState<T>;
}
