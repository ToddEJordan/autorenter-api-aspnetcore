using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AutoRenter.Api.Authorization
{
    public class IsAdminHandler : AuthorizationHandler<AdministratorAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorAuthorizationRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == AutoRenterClaimNames.IsAdministrator))
            {
                return Task.CompletedTask;
            }

            var isAdmin = Convert.ToBoolean(context.User.FindFirst(c => c.Type == AutoRenterClaimNames.IsAdministrator).Value);

            if (isAdmin)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
