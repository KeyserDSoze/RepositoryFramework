using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace RepositoryFramework.Web.Components.Services
{
    public interface IPolicyEvaluatorManager
    {
        ValueTask<bool> ValidateAsync(HttpContext httpContext, List<string> policies);
    }
    public class PolicyEvaluatorManager : IPolicyEvaluatorManager
    {
        private readonly IPolicyEvaluator _policyEvaluator;
        private readonly IAuthorizationPolicyProvider _policyProvider;

        public PolicyEvaluatorManager(
            IPolicyEvaluator policyEvaluator,
            IAuthorizationPolicyProvider policyProvider)
        {
            _policyEvaluator = policyEvaluator;
            _policyProvider = policyProvider;
        }
        public async ValueTask<bool> ValidateAsync(HttpContext httpContext, List<string> policies)
        {
            foreach (var policy in policies)
            {
                var providedPolicy = await _policyProvider.GetPolicyAsync(policy);
                if (!(await _policyEvaluator.AuthorizeAsync(providedPolicy!,
                        await _policyEvaluator.AuthenticateAsync(providedPolicy!, httpContext),
                        httpContext, null)).Succeeded)
                    return false;
            }
            return true;
        }
    }
}
