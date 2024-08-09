namespace Together.Application.Features.FeatureReply.Commands;

public sealed class DeleteReplyCommand(Guid id) : IBaseRequest
{
    private Guid Id { get; set; } = id;
    
    public class Validator : AbstractValidator<DeleteReplyCommand>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
    
    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<DeleteReplyCommand>(httpContextAccessor)
    {
        protected override async Task HandleAsync(DeleteReplyCommand request, CancellationToken ct)
        {
            var reply = await context.Replies.FirstOrDefaultAsync(r => r.Id == request.Id, ct);
            if (reply is null) throw new DomainException(TogetherErrorCodes.Reply.ReplyNotFound);

            context.Replies.Remove(reply);

            await context.SaveChangesAsync(ct);

            Message = "Deleted";
        }
    }
}