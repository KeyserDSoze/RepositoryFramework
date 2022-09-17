namespace RepositoryFramework
{
    public record BusinessType(RepositoryMethods Method, Type Service, bool IsAfterRequest = true);
}
