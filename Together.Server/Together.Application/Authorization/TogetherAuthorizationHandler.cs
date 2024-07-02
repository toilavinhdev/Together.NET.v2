using Microsoft.AspNetCore.Authorization;

namespace Together.Application.Authorization;

public sealed class TogetherAuthorizationHandler(IRedisService redisService) : AuthorizationHandler<TogetherPolicyRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, TogetherPolicyRequirement requirement)
    {
        if (context.User.Identity is {IsAuthenticated: false})
        {
            context.Fail();
            return;
        }

        var userId = context.User.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value;
        
        if (userId is null || !Guid.TryParse(userId, out _))
        {
            context.Fail();
            return;
        }

        var user = await redisService.GetAsync<IdentityPrivilege>(TogetherRedisKeys.GetIdentityPrivilegeKey(userId));

        var canAccess = user is not null && 
                        user.RoleClaims!.Any(claim => 
                            claim.Equals(requirement.Permission) || 
                            claim.Equals(TogetherPolicies.All));
        
        if (!canAccess)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}