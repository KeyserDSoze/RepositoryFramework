namespace RepositoryFramework
{
    public record Key<T1, T2, T3, T4>(T1 Primary, T2 Secondary, T3 Tertiary, T4 Quaternary) 
        : Key<T1, T2, T3>(Primary, Secondary, Tertiary)
       where T1 : notnull
       where T2 : notnull
       where T3 : notnull
       where T4 : notnull
    {
    }
}
