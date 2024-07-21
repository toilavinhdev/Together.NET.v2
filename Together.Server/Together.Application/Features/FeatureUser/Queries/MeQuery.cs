using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Queries;

public sealed class MeQuery : IBaseRequest<MeResponse>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, IRedisService redisService) 
        : BaseRequestHandler<MeQuery, MeResponse>(httpContextAccessor)
    {
        protected override async Task<MeResponse> HandleAsync(MeQuery request, CancellationToken ct)
        {
            var user = await redisService.GetAsync<IdentityPrivilege>(
                TogetherRedisKeys.IdentityPrivilegeKey(CurrentUserClaims.SubId));
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            return new MeResponse
            {
                Id = user.Id,
                SubId = user.SubId,
                Avatar = user.Avatar,
                Email = user.Email,
                Status = user.Status,
                UserName = user.UserName,
                Permissions = user.RoleClaims
            };
        }
    }
}