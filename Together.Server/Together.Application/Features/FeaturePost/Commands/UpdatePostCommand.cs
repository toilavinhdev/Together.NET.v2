namespace Together.Application.Features.FeaturePost.Commands;

public sealed class UpdatePostCommand : IBaseRequest
{
    public Guid Id { get; set; }
    
    public Guid TopicId { get; set; }
    
    public Guid? PrefixId { get; set; }
    
    public string Title { get; set; } = default!;

    public string Body { get; set; } = default!;
    
    public class Validator : AbstractValidator<UpdatePostCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.TopicId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<UpdatePostCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdatePostCommand request, CancellationToken ct)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == request.Id, ct); 
            if (post is null) throw new DomainException(TogetherErrorCodes.Post.PostNotFound);
            
            if (post.PrefixId != request.PrefixId &&
                !await context.Prefixes.AnyAsync(pf => pf.Id == request.PrefixId, ct))
                throw new DomainException(TogetherErrorCodes.Prefix.PrefixNotFound);

            if (post.TopicId != request.TopicId)
            {
                var topic = await context.Topics.FirstOrDefaultAsync(x => x.Id == request.TopicId, ct);
                if (topic is null) throw new DomainException(TogetherErrorCodes.Topic.TopicNotFound);
                post.TopicId = request.TopicId;
                post.ForumId = topic.ForumId;
            }

            post.PrefixId = request.PrefixId;
            post.Title = request.Title;
            post.Body = request.Body;
            post.MarkUserModified(CurrentUserClaims.Id);

            context.Posts.Update(post);

            await context.SaveChangesAsync(ct);

            Message = "Updated";
        }
    }
}