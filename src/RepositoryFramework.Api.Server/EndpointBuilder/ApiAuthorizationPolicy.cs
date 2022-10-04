namespace RepositoryFramework
{
    internal sealed class ApiAuthorizationPolicy : IApiAuthorizationPolicy
    {
        private readonly RepositoryMethods _method;
        private readonly ApiAuthorizationBuilder _authorizationBuilder;
        private readonly List<string> _policies = new();
        public ApiAuthorizationPolicy(RepositoryMethods method, ApiAuthorizationBuilder authorization)
        {
            _method = method;
            _authorizationBuilder = authorization;
        }
        public IApiAuthorizationPolicy With(params string[] policies)
        {
            foreach (var policy in policies)
                if (!_policies.Contains(policy))
                    _policies.Add(policy);
            return this;
        }
        public IApiAuthorizationBuilder Empty()
        {
            _authorizationBuilder.Authorization.Policies[_method] = Array.Empty<string>();
            return _authorizationBuilder;
        }
        public IApiAuthorizationBuilder And()
        {
            _authorizationBuilder.Authorization.Policies[_method] = _policies.ToArray();
            return _authorizationBuilder;
        }
    }
}
