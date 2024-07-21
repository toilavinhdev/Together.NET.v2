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

        var subId = context.User.Claims.FirstOrDefault(x => x.Type.Equals("subId"))?.Value;
        
        if (subId is null)
        {
            context.Fail();
            return;
        }

        var user = await redisService.GetAsync<IdentityPrivilege>(TogetherRedisKeys.IdentityPrivilegeKey(subId));

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