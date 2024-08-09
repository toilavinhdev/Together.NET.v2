using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Queries;

public sealed class MeQuery : IBaseRequest<MeResponse>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<MeQuery, MeResponse>(httpContextAccessor)
    {
        protected override async Task<MeResponse> HandleAsync(MeQuery request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == CurrentUserClaims.Id, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);

            var roles = await context.UserRoles
                .Include(r => r.Role)
                .Where(ur => ur.UserId == CurrentUserClaims.Id)
                .Select(ur => ur.Role)
                .ToListAsync(ct);

            return new MeResponse
            {
                Id = user.Id,
                SubId = user.SubId,
                Avatar = user.Avatar,
                Email = user.Email,
                Status = user.Status,
                UserName = user.UserName,
                Permissions = roles
                    .SelectMany(role => role.Claims)
                    .Distinct()
                    .OrderBy(role => role)
                    .ToList()
            };
        }
    }
}