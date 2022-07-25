namespace RepositoryFramework
{
    public class ApiAuthorizationPolicy
    {
        private readonly RepositoryMethod _method;
        private readonly ApiAuthorizationBuilder _authorizationBuilder;
        private readonly List<string> _policies = new();
        public ApiAuthorizationPolicy(RepositoryMethod method, ApiAuthorizationBuilder authorization)
        {
            _method = method;
            _authorizationBuilder = authorization;
        }
        public ApiAuthorizationPolicy With(string policy)
        {
            if (!_policies.Contains(policy))
                _policies.Add(policy);
            return this;
        }
        public ApiAuthorizationBuilder Empty()
        {
            _authorizationBuilder.Authorization.Policies[_method] = Array.Empty<string>();
            return _authorizationBuilder;
        }
        public ApiAuthorizationBuilder And()
        {
            _authorizationBuilder.Authorization.Policies[_method] = _policies.ToArray();
            return _authorizationBuilder;
        }
    }
}
