using Microsoft.AspNetCore.Authorization;

namespace Together.Application.Authorization;

public sealed class TogetherAuthorizationHandlerV2(IRedisService redisService) : AuthorizationHandler<TogetherPolicyRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, TogetherPolicyRequirement requirement)
    {
        if (context.User.Identity is {IsAuthenticated: false})
        {
            context.Fail();
            return;
        }

        var userId = context.User.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value.ToGuid();
        
        if (userId is null)
        {
            context.Fail();
            return;
        }

        var user = await redisService.StringGetAsync<IdentityPrivilege>(
            TogetherRedisKeys.IdentityPrivilegeKey(userId));

        var canAccess = user is not null && 
                        user.RoleClaims.Any(claim => 
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