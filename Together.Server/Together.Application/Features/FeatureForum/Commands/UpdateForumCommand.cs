namespace Together.Application.Features.FeatureForum.Commands;

public sealed class UpdateForumCommand : IBaseRequest
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public class Validator : AbstractValidator<UpdateForumCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<UpdateForumCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdateForumCommand request, CancellationToken ct)
        {
            var forum = await context.Forums.FirstOrDefaultAsync(f => f.Id == request.Id, ct);

            if (forum is null) throw new DomainException(TogetherErrorCodes.Forum.ForumNotFound);

            forum.Name = request.Name;
            forum.MarkUserModified(CurrentUserClaims.Id);

            context.Forums.Update(forum);

            await context.SaveChangesAsync(ct);

            Message = "Updated";
        }
    }
}