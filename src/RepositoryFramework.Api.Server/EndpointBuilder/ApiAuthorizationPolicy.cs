using Microsoft.AspNetCore.Routing;

namespace RepositoryFramework
{
    internal sealed class ApiAuthorizationPolicy : IApiAuthorizationPolicy
    {
        private readonly RepositoryMethods _method;
        private readonly ApiAuthorizationBuilder _authorizationBuilder;
        public ApiAuthorizationPolicy(RepositoryMethods method, ApiAuthorizationBuilder authorization)
        {
            _method = method;
            _authorizationBuilder = authorization;
        }
        public IApiAuthorizationPolicy With(params string[] policies)
        {
            foreach (var policy in policies)
                if (!_authorizationBuilder.Authorization.Policies[_method].Contains(policy))
                    _authorizationBuilder.Authorization.Policies[_method].Add(policy);
            return this;
        }
        public IApiAuthorizationBuilder Empty()
        {
            _authorizationBuilder.Authorization.Policies[_method] = new();
            return _authorizationBuilder;
        }
        public IApiAuthorizationBuilder And() 
            => _authorizationBuilder;
        public IEndpointRouteBuilder Build()
            => _authorizationBuilder.Build();
    }
}
