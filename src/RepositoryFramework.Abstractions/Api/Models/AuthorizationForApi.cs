namespace RepositoryFramework
{
    public class AuthorizationForApi
    {
        public ApiName Path { get; set; } = ApiName.Get | ApiName.Search | ApiName.Insert | ApiName.Update | ApiName.Delete;
        public string[]? Policies { get; set; }
    }
}
