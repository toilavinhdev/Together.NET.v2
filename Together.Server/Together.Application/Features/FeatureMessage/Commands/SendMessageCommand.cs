using Together.Application.Features.FeatureMessage.Responses;
using Together.Domain.Aggregates.MessageAggregate;

namespace Together.Application.Features.FeatureMessage.Commands;

public sealed class SendMessageCommand : IBaseRequest<SendMessageResponse>
{
    public Guid ConversationId { get; set; }

    public string Text { get; set; } = default!;
    
    public class Validator : AbstractValidator<SendMessageCommand>
    {
        public Validator()
        {
            RuleFor(x => x.ConversationId).NotEmpty();
            RuleFor(x => x.Text).NotEmpty();
        }
    }

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context) 
        : BaseRequestHandler<SendMessageCommand, SendMessageResponse>(httpContextAccessor)
    {
        protected override async Task<SendMessageResponse> HandleAsync(SendMessageCommand request, CancellationToken ct)
        {
            if (!await context.Conversations.AnyAsync(c => c.Id == request.ConversationId, ct))
                throw new DomainException(TogetherErrorCodes.Conversation.ConversationNotFound);

            if (!await context.ConversationParticipants.AnyAsync(cp =>
                    cp.ConversationId == request.ConversationId &&
                    cp.UserId == CurrentUserClaims.Id, ct))
                throw new DomainException(TogetherErrorCodes.Conversation.HaveNotJoinedConversation);

            var message = request.MapTo<Message>();
            message.MarkUserCreated(CurrentUserClaims.Id);

            await context.Messages.AddAsync(message, ct);

            await context.SaveChangesAsync(ct);

            return message.MapTo<SendMessageResponse>();
        }
    }
}