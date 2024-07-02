using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Queries;

public class MeQuery : IBaseRequest<MeResponse>
{
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<MeQuery, MeResponse>(httpContextAccessor)
    {
        protected override async Task<MeResponse> HandleAsync(MeQuery request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == CurrentUserClaims.Id, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);
            
            return user.MapTo<MeResponse>();
        }
    }
}