namespace Together.Application.Features.FeatureReply.Commands;

public sealed class UpdateReplyCommand : IBaseRequest
{
    public Guid Id { get; set; }
    
    public string Body { get; set; } = default!;
    
    public class Validator : AbstractValidator<UpdateReplyCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context)
        : BaseRequestHandler<UpdateReplyCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(UpdateReplyCommand request, CancellationToken ct)
        {
            var reply = await context.Replies.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
            if (reply is null) throw new DomainException(TogetherErrorCodes.Reply.ReplyNotFound);

            reply.Body = request.Body;
            reply.MarkUserCreated(CurrentUserClaims.Id);

            context.Replies.Update(reply);

            await context.SaveChangesAsync(ct);

            Message = "Updated";
        }
    }
}