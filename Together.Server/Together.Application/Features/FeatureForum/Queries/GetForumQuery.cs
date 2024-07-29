using Together.Application.Features.FeatureForum.Responses;

namespace Together.Application.Features.FeatureForum.Queries;

public sealed class GetForumQuery(Guid id) : IBaseRequest<GetForumResponse>
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<GetForumResponse>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<GetForumQuery, GetForumResponse>(httpContextAccessor)
    {
        protected override async Task<GetForumResponse> HandleAsync(GetForumQuery request, CancellationToken ct)
        {
            var forum = await context.Forums
                .Where(f => f.Id == request.Id)
                .Select(f => f.MapTo<GetForumResponse>())
                .FirstOrDefaultAsync(ct);

            if (forum is null) throw new DomainException(TogetherErrorCodes.Forum.ForumNotFound);

            return forum;
        }
    }
}