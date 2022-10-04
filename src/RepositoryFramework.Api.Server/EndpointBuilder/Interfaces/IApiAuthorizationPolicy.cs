namespace RepositoryFramework
{
    public interface IApiAuthorizationPolicy
    {
        IApiAuthorizationPolicy With(params string[] policies);
        IApiAuthorizationBuilder Empty();
        IApiAuthorizationBuilder And();
    }
}
