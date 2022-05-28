namespace RepositoryFramework
{
    public class AuthorizationForApi
    {
        public AuthorizationPath Path { get; set; } = AuthorizationPath.Get | AuthorizationPath.Query | AuthorizationPath.Insert | AuthorizationPath.Update | AuthorizationPath.Delete;
        public string[]? Policies { get; set; }
    }
}
