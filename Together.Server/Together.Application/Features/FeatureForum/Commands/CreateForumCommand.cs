using Together.Application.Features.FeatureForum.Responses;
using Together.Domain.Aggregates.ForumAggregate;

namespace Together.Application.Features.FeatureForum.Commands;

public sealed class CreateForumCommand : IBaseRequest<CreateForumResponse>
{
    public string Name { get; set; } = default!;
    
    public class Validator : AbstractValidator<CreateForumCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) : 
        BaseRequestHandler<CreateForumCommand, CreateForumResponse>(httpContextAccessor)
    {
        protected override async Task<CreateForumResponse> HandleAsync(CreateForumCommand request, CancellationToken ct)
        {
            var forum = request.MapTo<Forum>();
            forum.MarkUserCreated(CurrentUserClaims.Id);

            await context.AddAsync(forum, ct);
            
            await context.SaveChangesAsync(ct);

            Message = "Created";

            return forum.MapTo<CreateForumResponse>();
        }
    }
}