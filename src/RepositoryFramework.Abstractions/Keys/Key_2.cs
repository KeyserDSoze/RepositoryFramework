namespace RepositoryFramework
{
    public record Key<T1, T2>(T1 Primary, T2 Secondary)
        : Key<T1>(Primary)
        where T1 : notnull
        where T2 : notnull
    {
    }
}
