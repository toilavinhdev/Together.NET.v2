namespace Together.Application.Features.FeaturePost.Commands;

public sealed class DeletePostCommand(Guid id) : IBaseRequest
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<DeletePostCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<DeletePostCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeletePostCommand request, CancellationToken ct)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == request.Id, ct);
            if (post is null) throw new DomainException(TogetherErrorCodes.Post.PostNotFound);

            context.Posts.Remove(post);

            await context.SaveChangesAsync(ct);
            
            Message = "Deleted";
        }
    }
}