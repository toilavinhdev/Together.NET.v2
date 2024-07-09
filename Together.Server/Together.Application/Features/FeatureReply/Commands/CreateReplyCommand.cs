using Together.Application.Features.FeatureReply.Responses;
using Together.Domain.Aggregates.ReplyAggregate;

namespace Together.Application.Features.FeatureReply.Commands;

public sealed class CreateReplyCommand : IBaseRequest<CreateReplyResponse>
{
    public Guid PostId { get; set; }
    
    public Guid? ParentId { get; set; }
    
    public string Body { get; set; } = default!;
    
    public class Validator : AbstractValidator<CreateReplyCommand>
    {
        public Validator()
        {
            RuleFor(x => x.PostId).NotEmpty();
            RuleFor(x => x.Body).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<CreateReplyCommand, CreateReplyResponse>(httpContextAccessor)
    {
        protected override async Task<CreateReplyResponse> HandleAsync(CreateReplyCommand request, CancellationToken ct)
        {
            if (!await context.Posts.AnyAsync(p => p.Id == request.PostId, ct))
                throw new DomainException(TogetherErrorCodes.Post.PostNotFound);
            
            if (request.ParentId is not null && 
                !await context.Replies.AnyAsync(r => r.Id == request.ParentId && r.PostId == request.PostId, ct))
                throw new DomainException(TogetherErrorCodes.Reply.ReplyNotFound);

            var reply = request.MapTo<Reply>();
            reply.MarkUserCreated(CurrentUserClaims.Id);

            await context.Replies.AddAsync(reply, ct);

            await context.SaveChangesAsync(ct);

            return reply.MapTo<CreateReplyResponse>();
        }
    }
}