using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Queries;

public sealed class MeQuery : IBaseRequest<MeResponse>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, IRedisService redisService) 
        : BaseRequestHandler<MeQuery, MeResponse>(httpContextAccessor)
    {
        protected override async Task<MeResponse> HandleAsync(MeQuery request, CancellationToken ct)
        {
            var user = await redisService.StringGetAsync<IdentityPrivilege>(
                TogetherRedisKeys.IdentityPrivilegeKey(CurrentUserClaims.Id));
            
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            return new MeResponse
            {
                Id = user.Id,
                SubId = user.SubId,
                UserName = user.UserName,
                Avatar = user.Avatar,
                Email = user.Email,
                Status = user.Status,
            };
        }
    }
}