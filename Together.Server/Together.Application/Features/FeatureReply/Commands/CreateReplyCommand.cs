using Together.Application.Features.FeatureNotification.Commands;
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
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, ISender sender) 
        : BaseRequestHandler<CreateReplyCommand, CreateReplyResponse>(httpContextAccessor)
    {
        protected override async Task<CreateReplyResponse> HandleAsync(CreateReplyCommand request, CancellationToken ct)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId, ct);
            if (post is null) throw new DomainException(TogetherErrorCodes.Post.PostNotFound);
            
            if (request.ParentId is not null && 
                !await context.Replies.AnyAsync(r => r.Id == request.ParentId && r.PostId == request.PostId, ct))
                throw new DomainException(TogetherErrorCodes.Reply.ReplyNotFound);

            var reply = request.MapTo<Reply>();
            reply.MarkUserCreated(CurrentUserClaims.Id);

            await context.Replies.AddAsync(reply, ct);

            await context.SaveChangesAsync(ct);

            if (request.ParentId is null)
            {
                if (post.CreatedById != CurrentUserClaims.Id)
                {
                    await sender.Send(new SendNotificationCommand
                    {
                        ActorId = CurrentUserClaims.Id,
                        ReceiverId = post.CreatedById,
                        SourceId = reply.Id,
                        Activity = NotificationActivity.REPLY_POST
                    }, ct);
                }
            }
            else
            {
                var parentReply = await context.Replies.FirstOrDefaultAsync(r => r.Id == reply.ParentId, ct);
                if (reply.ParentId is not null && parentReply!.CreatedById != CurrentUserClaims.Id)
                {
                    await sender.Send(new SendNotificationCommand
                    {
                        ActorId = CurrentUserClaims.Id,
                        ReceiverId = parentReply!.CreatedById,
                        SourceId = reply.Id,
                        Activity = NotificationActivity.REPLY_REPLY
                    }, ct);
                }
            }
            

            return reply.MapTo<CreateReplyResponse>();
        }
    }
}