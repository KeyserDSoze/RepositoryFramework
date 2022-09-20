namespace RepositoryFramework
{
    public record Key<T1, T2, T3>(T1 Primary, T2 Secondary, T3 Tertiary) : Key<T1, T2>(Primary, Secondary), IKey
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        public new string AsString()
            => GetStringedValues(Primary, Secondary, Tertiary);
    }
}
