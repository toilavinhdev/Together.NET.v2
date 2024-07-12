using Together.Application.Features.FeatureMessage.Responses;
using Together.Application.Sockets;
using Together.Application.Sockets.WebSocketMessages;
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

    internal class Handler(IHttpContextAccessor httpContextAccessor, TogetherContext context, TogetherWebSocketHandler socket) 
        : BaseRequestHandler<SendMessageCommand, SendMessageResponse>(httpContextAccessor)
    {
        protected override async Task<SendMessageResponse> HandleAsync(SendMessageCommand request, CancellationToken ct)
        {
            var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Id == CurrentUserClaims.Id, ct);
            if (currentUser is null) throw new DomainException(TogetherErrorCodes.User.UserNotFound);
            
            if (!await context.Conversations.AnyAsync(c => c.Id == request.ConversationId, ct))
                throw new DomainException(TogetherErrorCodes.Conversation.ConversationNotFound);

            if (!await context.ConversationParticipants.AnyAsync(cp =>
                    cp.ConversationId == request.ConversationId &&
                    cp.UserId == CurrentUserClaims.Id, ct))
                throw new DomainException(TogetherErrorCodes.Conversation.HaveNotJoinedConversation);

            var message = request.MapTo<Message>();
            message.MarkUserCreated(currentUser.Id);

            await context.Messages.AddAsync(message, ct);

            await context.SaveChangesAsync(ct);

            await SocketHandlerAsync(message, ct);
            
            return message.MapTo<SendMessageResponse>();
        }

        private async Task SocketHandlerAsync(Message message, CancellationToken ct)
        {
            var participantIds = await context.ConversationParticipants
                .Where(cp => cp.ConversationId == message.ConversationId && cp.UserId != CurrentUserClaims.Id)
                .Select(cp => cp.UserId)
                .ToListAsync(ct);

            foreach (var participantId in participantIds)
            {
                await socket.SendMessageAsync(
                    participantId.ToString(),
                    new WebSocketMessage<ReceivedMessageWebSocketMessage>
                    {
                        Target = WebSocketClientTarget.ReceivedMessage,
                        Message = message.MapTo<ReceivedMessageWebSocketMessage>()
                    });
            }
        }
    }
}