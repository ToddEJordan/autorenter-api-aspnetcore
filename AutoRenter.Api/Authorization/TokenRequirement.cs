using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutoRenter.Api.Authorization
{
    public class TokenRequirement : AuthorizationHandler<TokenRequirement>, IAuthorizationRequirement
    {
        private readonly ITokenManager tokenManager;

        public TokenRequirement(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext mvcContext)
            {
                var token = mvcContext.HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(token) && tokenManager.IsTokenValid(token))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
