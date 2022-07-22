namespace RepositoryFramework
{
    public record Key<T1>(T1 Primary) 
        where T1 : notnull
    { 
    }
}
