namespace RepositoryFramework
{
    public record Key<T1, T2, T3, T4, T5>(T1 Primary, T2 Secondary, T3 Tertiary, T4 Quaternary, T5 Quinary) 
        : Key<T1, T2, T3, T4>(Primary, Secondary, Tertiary, Quaternary)
       where T1 : notnull
       where T2 : notnull
       where T3 : notnull
       where T4 : notnull
       where T5 : notnull
    {
    }
}
