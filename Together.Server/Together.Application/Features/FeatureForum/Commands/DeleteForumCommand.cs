namespace Together.Application.Features.FeatureForum.Commands;

public sealed class DeleteForumCommand(Guid id) : IBaseRequest
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<DeleteForumCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<DeleteForumCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeleteForumCommand request, CancellationToken ct)
        {
            var forum = await context.Forums.FirstOrDefaultAsync(f => f.Id == request.Id, ct);

            if (forum is null) throw new DomainException(TogetherErrorCodes.Forum.ForumNotFound);

            context.Forums.Remove(forum);

            await context.SaveChangesAsync(ct);

            Message = "Deleted";
        }
    }
}