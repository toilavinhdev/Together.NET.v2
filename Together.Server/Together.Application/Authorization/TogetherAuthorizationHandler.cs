using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Together.Application.Authorization;

public class TogetherAuthorizationHandler(IServiceProvider serviceProvider) : AuthorizationHandler<TogetherPolicyRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, TogetherPolicyRequirement requirement)
    {
        using var scope = serviceProvider.CreateScope();
        var togetherContext = scope.ServiceProvider.GetRequiredService<TogetherContext>();
        
        if (!context.User.Identity!.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        var userId = context.User.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value.ToGuid();

        var users = await togetherContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        
        if (users is null)
        {
            context.Fail();
            return;
        }

        var hasPermission = await togetherContext.UserRoles
            .Include(r => r.Role)
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .AnyAsync(r => r.Claims
                .Any(claim => 
                    claim == requirement.Permission || 
                    claim == TogetherPolicies.All));

        if (!hasPermission)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}