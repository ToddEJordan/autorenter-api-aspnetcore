using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AutoRenter.Api.Authorization
{
    public class IsAdminHandler : AuthorizationHandler<IsAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdminRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "is_administrator"))
            {
                return Task.CompletedTask;
            }

            var isAdmin = Convert.ToBoolean(context.User.FindFirst(c => c.Type == "is_administrator").Value);

            if (isAdmin)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
