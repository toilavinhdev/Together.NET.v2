using Together.Application.Features.FeatureUser.Responses;

namespace Together.Application.Features.FeatureUser.Queries;

public class GetUserQuery(Guid userId) : IBaseRequest<GetUserResponse>
{
    private Guid UserId { get; set; } = userId;
    
    public class Validator : AbstractValidator<GetUserQuery>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) :
        BaseRequestHandler<GetUserQuery, GetUserResponse>(httpContextAccessor)
    {
        protected override async Task<GetUserResponse> HandleAsync(GetUserQuery request, CancellationToken ct)
        {
            var user = await context.Users
                .Select(u => u.MapTo<GetUserResponse>())
                .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);
            if (user is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);
            
            return user;
        }
    }
}